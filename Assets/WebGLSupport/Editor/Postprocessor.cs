using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace WebGLSupport
{
    public class Postprocessor
    {
        // Ruta del menú en el editor
        const string MenuPath = "Assets/WebGLSupport/OverwriteFullscreenButton";

#if UNITY_2021_1_OR_NEWER
        // Variables para versiones recientes de Unity
        static readonly bool supportedPostprocessor = true;
        static readonly string defaultFullscreenFunc = "unityInstance.SetFullscreen(1);";
        static readonly string fullscreenNode = "unity-container";
#else
        // Variables para versiones antiguas de Unity
        static readonly bool supportedPostprocessor = false;
        static readonly string defaultFullscreenFunc = "";
        static readonly string fullscreenNode = "";
#endif

        // Propiedad para verificar si la opción está habilitada
        private static bool IsEnable => EditorPrefs.GetInt(MenuPath, 1) == 1;

        // Función llamada después de realizar la construcción
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.WebGL) return;
            if (!supportedPostprocessor) return;
            if (!IsEnable) return;

            // Ruta al archivo index.html generado
            var path = Path.Combine(pathToBuiltProject, "index.html");
            if (!File.Exists(path)) return;

            var html = File.ReadAllText(path);

            // Verificar si el nodo existe en el HTML
            if (html.Contains(fullscreenNode))
            {
                // Reemplazar la función predeterminada por el código personalizado
                html = html.Replace(defaultFullscreenFunc, $"document.makeFullscreen('{fullscreenNode}');");
                File.WriteAllText(path, html);
            }
        }

        // Función del menú para alternar el estado de la opción de pantalla completa
        [MenuItem(MenuPath)]
        public static void OverwriteDefaultFullscreenButton()
        {
            var flag = !IsEnable; // Alternar estado
            EditorPrefs.SetInt(MenuPath, flag ? 1 : 0); // Guardar preferencia
            Debug.Log($"Fullscreen Button Enabled: {flag}");
        }

        // Validar el estado del menú
        [MenuItem(MenuPath, validate = true)]
        private static bool OverwriteDefaultFullscreenButtonValidator()
        {
            return true; // No es necesario cambiar nada aquí, ya que el estado se maneja internamente
        }
    }
}
