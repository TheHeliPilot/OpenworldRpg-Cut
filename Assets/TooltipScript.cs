using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipScript : MonoBehaviour
{
        public TextMeshProUGUI headerField;
        public TextMeshProUGUI contentField;
        public LayoutElement layoutElement;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetText(string text, string header = "")
        {
            headerField.gameObject.SetActive(!string.IsNullOrEmpty(header));
            headerField.text = header;
            contentField.text = text;
                
            layoutElement.enabled = Math.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElement.preferredWidth;
        }

        private void Update()
        {
            Vector2 mousePos = Input.mousePosition;

            float pX = mousePos.x / Screen.width;
            float pY = mousePos.y / Screen.height;

            _rectTransform.pivot = new Vector2(pX, pY);
            transform.position = new Vector3(mousePos.x, mousePos.y + 20, 0);
        }
}
