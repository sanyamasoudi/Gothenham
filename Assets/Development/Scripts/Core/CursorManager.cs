using System;
using UnityEngine;

namespace Development.Scripts.Core
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private Texture2D cursorTexture;
        [SerializeField] private Vector2 cursorHotSpot;

        private void Awake()
        {
            Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.ForceSoftware);
        }
    }
}