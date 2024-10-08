using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace NaughtyAttributes.Editor
{
  public static class NaughtyEditorGUI
  {
    public const float IndentLength = 15.0f;
    public const float HorizontalSpacing = 2.0f;

    private static readonly GUIStyle _buttonStyle = new(GUI.skin.button) { richText = true };

    public static void PropertyField(Rect rect, SerializedProperty property, bool includeChildren)
    {
      PropertyField_Implementation(rect, property, includeChildren, DrawPropertyField);
    }

    public static void PropertyField_Layout(SerializedProperty property, bool includeChildren)
    {
      var dummyRect = new Rect();
      PropertyField_Implementation(dummyRect, property, includeChildren, DrawPropertyField_Layout);
    }

    private static void DrawPropertyField(Rect rect, SerializedProperty property, GUIContent label,
      bool includeChildren)
    {
      EditorGUI.PropertyField(rect, property, label, includeChildren);
    }

    private static void DrawPropertyField_Layout(Rect rect, SerializedProperty property, GUIContent label,
      bool includeChildren)
    {
      EditorGUILayout.PropertyField(property, label, includeChildren);
    }

    private static void PropertyField_Implementation(Rect rect, SerializedProperty property, bool includeChildren,
      PropertyFieldFunction propertyFieldFunction)
    {
      var specialCaseAttribute = PropertyUtility.GetAttribute<SpecialCaseDrawerAttribute>(property);
      if (specialCaseAttribute != null)
      {
        specialCaseAttribute.GetDrawer().OnGUI(rect, property);
      }
      else
      {
        // Check if visible
        var visible = PropertyUtility.IsVisible(property);
        if (!visible) return;

        // Validate
        var validatorAttributes = PropertyUtility.GetAttributes<ValidatorAttribute>(property);
        foreach (var validatorAttribute in validatorAttributes)
          validatorAttribute.GetValidator().ValidateProperty(property);

        // Check if enabled and draw
        EditorGUI.BeginChangeCheck();
        var enabled = PropertyUtility.IsEnabled(property);

        using (new EditorGUI.DisabledScope(!enabled))
        {
          propertyFieldFunction.Invoke(rect, property, PropertyUtility.GetLabel(property), includeChildren);
        }

        // Call OnValueChanged callbacks
        if (EditorGUI.EndChangeCheck()) PropertyUtility.CallOnValueChangedCallbacks(property);
      }
    }

    public static float GetIndentLength(Rect sourceRect)
    {
      var indentRect = EditorGUI.IndentedRect(sourceRect);
      var indentLength = indentRect.x - sourceRect.x;

      return indentLength;
    }

    public static void BeginBoxGroup_Layout(string label = "")
    {
      EditorGUILayout.BeginVertical(GUI.skin.box);
      if (!string.IsNullOrEmpty(label)) EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
    }

    public static void EndBoxGroup_Layout()
    {
      EditorGUILayout.EndVertical();
    }

    /// <summary>
    ///   Creates a dropdown
    /// </summary>
    /// <param name="rect">The rect the defines the position and size of the dropdown in the inspector</param>
    /// <param name="serializedObject">The serialized object that is being updated</param>
    /// <param name="target">The target object that contains the dropdown</param>
    /// <param name="dropdownField">The field of the target object that holds the currently selected dropdown value</param>
    /// <param name="label">The label of the dropdown</param>
    /// <param name="selectedValueIndex">The index of the value from the values array</param>
    /// <param name="values">The values of the dropdown</param>
    /// <param name="displayOptions">The display options for the values</param>
    public static void Dropdown(
      Rect rect, SerializedObject serializedObject, object target, FieldInfo dropdownField,
      string label, int selectedValueIndex, object[] values, string[] displayOptions)
    {
      EditorGUI.BeginChangeCheck();

      var newIndex = EditorGUI.Popup(rect, label, selectedValueIndex, displayOptions);
      var newValue = values[newIndex];

      var dropdownValue = dropdownField.GetValue(target);
      if (dropdownValue == null || !dropdownValue.Equals(newValue))
      {
        Undo.RecordObject(serializedObject.targetObject, "Dropdown");

        // TODO: Problem with structs, because they are value type.
        // The solution is to make boxing/unboxing but unfortunately I don't know the compile time type of the target object
        dropdownField.SetValue(target, newValue);
      }
    }

    public static void Button(Object target, MethodInfo methodInfo)
    {
      var visible = ButtonUtility.IsVisible(target, methodInfo);
      if (!visible) return;

      if (methodInfo.GetParameters().All(p => p.IsOptional))
      {
        var buttonAttribute = (ButtonAttribute)methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
        var buttonText = string.IsNullOrEmpty(buttonAttribute.Text)
          ? ObjectNames.NicifyVariableName(methodInfo.Name)
          : buttonAttribute.Text;

        var buttonEnabled = ButtonUtility.IsEnabled(target, methodInfo);

        var mode = buttonAttribute.SelectedEnableMode;
        buttonEnabled &=
          mode == EButtonEnableMode.Always ||
          (mode == EButtonEnableMode.Editor && !Application.isPlaying) ||
          (mode == EButtonEnableMode.Playmode && Application.isPlaying);

        var methodIsCoroutine = methodInfo.ReturnType == typeof(IEnumerator);
        if (methodIsCoroutine) buttonEnabled &= Application.isPlaying ? true : false;

        EditorGUI.BeginDisabledGroup(!buttonEnabled);

        if (GUILayout.Button(buttonText, _buttonStyle))
        {
          var defaultParams = methodInfo.GetParameters().Select(p => p.DefaultValue).ToArray();
          var methodResult = methodInfo.Invoke(target, defaultParams) as IEnumerator;

          if (!Application.isPlaying)
          {
            // Set target object and scene dirty to serialize changes to disk
            EditorUtility.SetDirty(target);

            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            if (stage != null)
              // Prefab mode
              EditorSceneManager.MarkSceneDirty(stage.scene);
            else
              // Normal scene
              EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
          }
          else if (methodResult != null && target is MonoBehaviour behaviour)
          {
            behaviour.StartCoroutine(methodResult);
          }
        }

        EditorGUI.EndDisabledGroup();
      }
      else
      {
        var warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
        HelpBox_Layout(warning, MessageType.Warning, target, true);
      }
    }

    public static void NativeProperty_Layout(Object target, PropertyInfo property)
    {
      var value = property.GetValue(target, null);

      if (value == null)
      {
        var warning = string.Format("{0} is null. {1} doesn't support reference types with null value",
          ObjectNames.NicifyVariableName(property.Name), typeof(ShowNativePropertyAttribute).Name);
        HelpBox_Layout(warning, MessageType.Warning, target);
      }
      else if (!Field_Layout(value, ObjectNames.NicifyVariableName(property.Name)))
      {
        var warning = string.Format("{0} doesn't support {1} types", typeof(ShowNativePropertyAttribute).Name,
          property.PropertyType.Name);
        HelpBox_Layout(warning, MessageType.Warning, target);
      }
    }

    public static void NonSerializedField_Layout(Object target, FieldInfo field)
    {
      var value = field.GetValue(target);

      if (value == null)
      {
        var warning = string.Format("{0} is null. {1} doesn't support reference types with null value",
          ObjectNames.NicifyVariableName(field.Name), typeof(ShowNonSerializedFieldAttribute).Name);
        HelpBox_Layout(warning, MessageType.Warning, target);
      }
      else if (!Field_Layout(value, ObjectNames.NicifyVariableName(field.Name)))
      {
        var warning = string.Format("{0} doesn't support {1} types", typeof(ShowNonSerializedFieldAttribute).Name,
          field.FieldType.Name);
        HelpBox_Layout(warning, MessageType.Warning, target);
      }
    }

    public static void HorizontalLine(Rect rect, float height, Color color)
    {
      rect.height = height;
      EditorGUI.DrawRect(rect, color);
    }

    public static void HelpBox(Rect rect, string message, MessageType type, Object context = null,
      bool logToConsole = false)
    {
      EditorGUI.HelpBox(rect, message, type);

      if (logToConsole) DebugLogMessage(message, type, context);
    }

    public static void HelpBox_Layout(string message, MessageType type, Object context = null,
      bool logToConsole = false)
    {
      EditorGUILayout.HelpBox(message, type);

      if (logToConsole) DebugLogMessage(message, type, context);
    }

    public static bool Field_Layout(object value, string label)
    {
      using (new EditorGUI.DisabledScope(true))
      {
        var isDrawn = true;
        var valueType = value.GetType();

        if (valueType == typeof(bool))
          EditorGUILayout.Toggle(label, (bool)value);
        else if (valueType == typeof(short))
          EditorGUILayout.IntField(label, (short)value);
        else if (valueType == typeof(ushort))
          EditorGUILayout.IntField(label, (ushort)value);
        else if (valueType == typeof(int))
          EditorGUILayout.IntField(label, (int)value);
        else if (valueType == typeof(uint))
          EditorGUILayout.LongField(label, (uint)value);
        else if (valueType == typeof(long))
          EditorGUILayout.LongField(label, (long)value);
        else if (valueType == typeof(ulong))
          EditorGUILayout.TextField(label, ((ulong)value).ToString());
        else if (valueType == typeof(float))
          EditorGUILayout.FloatField(label, (float)value);
        else if (valueType == typeof(double))
          EditorGUILayout.DoubleField(label, (double)value);
        else if (valueType == typeof(string))
          EditorGUILayout.TextField(label, (string)value);
        else if (valueType == typeof(Vector2))
          EditorGUILayout.Vector2Field(label, (Vector2)value);
        else if (valueType == typeof(Vector3))
          EditorGUILayout.Vector3Field(label, (Vector3)value);
        else if (valueType == typeof(Vector4))
          EditorGUILayout.Vector4Field(label, (Vector4)value);
        else if (valueType == typeof(Vector2Int))
          EditorGUILayout.Vector2IntField(label, (Vector2Int)value);
        else if (valueType == typeof(Vector3Int))
          EditorGUILayout.Vector3IntField(label, (Vector3Int)value);
        else if (valueType == typeof(Color))
          EditorGUILayout.ColorField(label, (Color)value);
        else if (valueType == typeof(Bounds))
          EditorGUILayout.BoundsField(label, (Bounds)value);
        else if (valueType == typeof(Rect))
          EditorGUILayout.RectField(label, (Rect)value);
        else if (valueType == typeof(RectInt))
          EditorGUILayout.RectIntField(label, (RectInt)value);
        else if (typeof(Object).IsAssignableFrom(valueType))
          EditorGUILayout.ObjectField(label, (Object)value, valueType, true);
        else if (valueType.BaseType == typeof(Enum))
          EditorGUILayout.EnumPopup(label, (Enum)value);
        else if (valueType.BaseType == typeof(TypeInfo))
          EditorGUILayout.TextField(label, value.ToString());
        else
          isDrawn = false;

        return isDrawn;
      }
    }

    private static void DebugLogMessage(string message, MessageType type, Object context)
    {
      switch (type)
      {
        case MessageType.None:
        case MessageType.Info:
          Debug.Log(message, context);
          break;
        case MessageType.Warning:
          Debug.LogWarning(message, context);
          break;
        case MessageType.Error:
          Debug.LogError(message, context);
          break;
      }
    }

    private delegate void PropertyFieldFunction(Rect rect, SerializedProperty property, GUIContent label,
      bool includeChildren);
  }
}