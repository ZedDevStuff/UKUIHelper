﻿using BepInEx;
using BepInEx.Configuration;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore;
using UnityEngine.UI;
using TMPro;

namespace UKUIHelper
{
    [BepInPlugin("zed.uk.uihelper", "UK UI Helper", "1.0.0")]
    public class UIHelper : BaseUnityPlugin
    {
        GameObject _button,_text,_panel,_image,_toggle,_scrollview,_dropdown,_inputField,_slider,_scrollbar;
        Sprite sprite;
        public GameObject button
        {
            get
            {
                return _button;
            }
            private set
            {
                _button = value;
            }
        }
        public GameObject text
        {
            get
            {
                return _text;
            }
            private set
            {
                _text = value;
            }
        }
        public GameObject panel
        {
            get
            {
                return _panel;
            }
            private set
            {
                _panel = value;
            }
        }
        public GameObject image
        {
            get
            {
                return _image;
            }
            private set
            {
                _image = value;
            }
        }
        public GameObject toggle
        {
            get
            {
                return _toggle;
            }
            private set
            {
                _toggle = value;
            }
        }
        public GameObject scrollview
        {
            get
            {
                return _scrollview;
            }
            private set
            {
                _scrollview = value;
            }
        }
        public GameObject dropdown
        {
            get
            {
                return _dropdown;
            }
            private set
            {
                _dropdown = value;
            }
        }
        public GameObject inputField
        {
            get
            {
                return _inputField;
            }
            private set
            {
                _inputField = value;
            }
        }
        public GameObject slider
        {
            get
            {
                return _slider;
            }
            private set
            {
                _slider = value;
            }
        }
        public GameObject scrollbar
        {
            get
            {
                return _scrollbar;
            }
            private set
            {
                _scrollbar = value;
            }
        }
        public Sprite Uisprite 
        {
            get
            {
                return sprite;
            }
            private set
            {
                sprite = value;
            }
        }
        TMP_FontAsset _TMPfont;
        Font _font;
        public TMP_FontAsset TMPfont
        {
            get
            {
                return _TMPfont;
            }
            private set
            {
                _TMPfont = value;
            }
        }
        public Font font
        {
            get
            {
                return _font;
            }
            private set
            {
                _font = value;
            }
        }
        private static UIHelper instance;

        private string path,workDir;
        private bool isTMPPresent = false;
        private ConfigEntry<bool> tryUseTMP;
        private void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += Scene;
            instance = this;
            tryUseTMP = Config.Bind("UK UI Helper", "Try TMP", false, "Try to use TextMeshPro if available. WARNING: This doesn't work right now.");
            path = System.Reflection.Assembly.GetAssembly(typeof(UIHelper)).Location;
            workDir = Path.GetDirectoryName(path);
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll",SearchOption.AllDirectories);
            if(tryUseTMP.Value)
            {
                Logger.LogInfo("Trying to use TextMeshPro");
                foreach (string file in files)
                {
                    if(file.Contains("Unity.TextMeshPro"))
                    {
                        isTMPPresent = true;
                    }
                }
                if(isTMPPresent)
                {
                    Logger.LogInfo("TMP is present, proceeding");
                    Logger.LogInfo("Loading TMP font...");
                    TMPfont = TMP_FontAsset.CreateFontAsset(Resources.GetBuiltinResource<Font>("Arial.ttf"));
                    Logger.LogInfo("Done");
                }
                else
                {
                    Logger.LogInfo("Could not find TMP, proceeding");
                    Logger.LogInfo("Loading font...");
                    font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    Logger.LogInfo("Done");
                }
            }
            else
            {
                Logger.LogInfo("Ignoring TMP, proceeding");
                isTMPPresent = false;
                Logger.LogInfo("Loading font...");
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                Logger.LogInfo("Done");
            }
            
            
            Logger.LogInfo("Loading sprite");
            Texture2D texture = new Texture2D(200, 200);
            texture.LoadImage(File.ReadAllBytes(workDir + "\\Sprite.png")); 
            sprite = Sprite.Create(texture, new Rect(0, 0, 200, 200), new Vector2(0.5f, 0.5f),200f,0,SpriteMeshType.Tight,new Vector4(15,15,15,15));
            Logger.LogInfo("Sprite loaded");
        }
        void Scene(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
        {
            if(scene.name.Contains("Menu"))
            {
                Logger.LogInfo("Main scene loaded");
            }
        }
        public static Sprite LoadSprite(Vector4 border)
        {
            Texture2D texture = new Texture2D(200, 200);
            texture.LoadImage(File.ReadAllBytes(instance.workDir + "\\Sprite.png"));
            return Sprite.Create(texture, new Rect(0, 0,texture.width,texture.height),new Vector2(0.5f, 0.5f),100f,0,SpriteMeshType.Tight,border);
        }
        public static GameObject CreateButton()
        {
            bool tmp = instance.isTMPPresent;
            GameObject blank = new GameObject();
            blank.name = "Button";
            blank.AddComponent<RectTransform>();
            blank.AddComponent<CanvasRenderer>();
            blank.AddComponent<Image>();
            blank.AddComponent<Button>();
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,50);
            blank.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);
            blank.GetComponent<Image>().sprite = instance.sprite;
            blank.GetComponent<Image>().type = Image.Type.Sliced;
            blank.GetComponent<Button>().targetGraphic = blank.GetComponent<Image>();
            GameObject text = CreateText();
            text.name = "Text";
            text.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            text.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            text.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
            text.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            if(tmp)
            {
                text.GetComponent<TMP_Text>().text = "Button";
                text.GetComponent<TMP_Text>().fontSize = 32;
                text.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Center;
                text.GetComponent<TMP_Text>().color = Color.black;

            }
            else
            {
                text.GetComponent<Text>().text = "Button";
                text.GetComponent<Text>().fontSize = 32;
                text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                text.GetComponent<Text>().color = Color.black;
            }
            return blank;
        }
        public static GameObject CreateText()
        {
            bool tmp = instance.isTMPPresent;
            GameObject blank = new GameObject();
            blank.name = "Text";
            blank.AddComponent<RectTransform>();
            blank.AddComponent<CanvasRenderer>();
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,50);
            blank.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);
            if(tmp)
            {
                blank.AddComponent<TextMeshProUGUI>();
                blank.GetComponent<TMP_Text>().text = "Text";
                blank.GetComponent<TMP_Text>().font = instance.TMPfont;
                blank.GetComponent<TMP_Text>().fontSize = 32;
                blank.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Center;
                blank.GetComponent<TMP_Text>().color = Color.black;
            }
            else
            {
                blank.AddComponent<Text>();
                blank.GetComponent<Text>().text = "Text";
                blank.GetComponent<Text>().font = instance.font;
                blank.GetComponent<Text>().fontSize = 32;
                blank.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                blank.GetComponent<Text>().color = Color.black;
            }
            return blank;
        }
        /*public static GameObject CreateDropdown()
        {
            bool tmp = instance.isTMPPresent;
            GameObject blank = new GameObject();
            blank.name = "Dropdown";
            blank.AddComponent<RectTransform>();
            blank.AddComponent<CanvasRenderer>();
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,50);
            blank.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);
            if(tmp)
            {

            }
            else
            {
                
            }
            return blank;
        }*/
        public static GameObject CreateInputField()
        {
            bool tmp = instance.isTMPPresent;
            GameObject blank = CreateImage();
            blank.name = "InputField";
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,40);
            blank.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);
            
            GameObject placeholder = CreateText();
            placeholder.name = "Placeholder";
            placeholder.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            placeholder.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            placeholder.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
            placeholder.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            placeholder.GetComponent<RectTransform>().sizeDelta = new Vector2(-20,0);

            GameObject text = CreateText();
            text.name = "Text";
            text.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            text.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            text.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
            text.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(-20,0);

            if(tmp)
            {
                placeholder.GetComponent<TMP_Text>().fontSize = 20;
                placeholder.GetComponent<TMP_Text>().fontStyle = TMPro.FontStyles.Italic;
                placeholder.GetComponent<TMP_Text>().color = new Color(0,0,0,0.5f);
                placeholder.GetComponent<TMP_Text>().text = "Enter text...";

                text.GetComponent<TMP_Text>().fontSize = 20;
                text.GetComponent<TMP_Text>().fontStyle = TMPro.FontStyles.Normal;
                text.GetComponent<TMP_Text>().color = new Color(0,0,0,1);
                text.GetComponent<TMP_Text>().text = "";

                blank.AddComponent<TMP_InputField>();
                blank.GetComponent<TMP_InputField>().textComponent = text.GetComponent<TMP_Text>();
                blank.GetComponent<TMP_InputField>().placeholder = placeholder.GetComponent<TMP_Text>();
                blank.GetComponent<TMP_InputField>().textViewport = blank.GetComponent<RectTransform>();
                blank.GetComponent<TMP_InputField>().targetGraphic = blank.GetComponent<Image>();
            }
            else
            {
                placeholder.GetComponent<Text>().fontSize = 20;
                placeholder.GetComponent<Text>().fontStyle = FontStyle.Italic;
                placeholder.GetComponent<Text>().color = new Color(0,0,0,0.5f);
                placeholder.GetComponent<Text>().text = "Enter text...";

                text.GetComponent<Text>().fontSize = 20;
                text.GetComponent<Text>().fontStyle = FontStyle.Normal;
                text.GetComponent<Text>().color = new Color(0,0,0,1);
                text.GetComponent<Text>().text = "";

                blank.AddComponent<InputField>();
                blank.GetComponent<InputField>().textComponent = text.GetComponent<Text>();
                blank.GetComponent<InputField>().placeholder = placeholder.GetComponent<Text>();
                blank.GetComponent<InputField>().targetGraphic = blank.GetComponent<Image>();
            }
            return blank;
        }
        public static GameObject CreatePanel()
        {
            GameObject blank = CreateImage();
            blank.name = "Panel";
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,200);
            blank.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);
            blank.GetComponent<Image>().color = Color.white;
            return blank;
        }
        public static GameObject CreateImage()
        {
            GameObject blank = new GameObject();
            blank.name = "Image";
            blank.AddComponent<RectTransform>();
            blank.AddComponent<CanvasRenderer>();
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,200);
            blank.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);
            blank.AddComponent<Image>();
            blank.GetComponent<Image>().sprite = instance.sprite;
            blank.GetComponent<Image>().type = Image.Type.Sliced;
            return blank;
        }
        public static GameObject CreateToggle()
        {
            GameObject blank = new GameObject();
            blank.name = "Toggle";
            blank.AddComponent<RectTransform>();
            blank.AddComponent<Toggle>();
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(30,30);
            blank.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);

            GameObject background = CreateImage();
            background.name = "Background";
            background.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            background.GetComponent<RectTransform>().sizeDelta = new Vector2(30,30);
            background.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            background.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            background.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);

            GameObject checkmark = CreateImage();
            checkmark.name = "Checkmark";
            checkmark.GetComponent<RectTransform>().SetParent(background.GetComponent<RectTransform>());
            checkmark.GetComponent<RectTransform>().sizeDelta = new Vector2(-14,-14);
            checkmark.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            checkmark.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            checkmark.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
            checkmark.GetComponent<Image>().color = Color.black;

            blank.GetComponent<Toggle>().targetGraphic = background.GetComponent<Image>();
            blank.GetComponent<Toggle>().graphic = checkmark.GetComponent<Image>();
            blank.GetComponent<Toggle>().isOn = true;
            return blank;
        }
        /*public static GameObject CreateScrollView()
        {
            GameObject blank = new GameObject();
            blank.name = "ScrollView";
            blank.AddComponent<RectTransform>();
            blank.AddComponent<CanvasRenderer>();
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,50);
            blank.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);
            return blank;
        }*/
        public static GameObject CreateSlider()
        {
            GameObject blank = new GameObject();
            blank.name = "Slider";
            blank.AddComponent<RectTransform>();
            blank.AddComponent<CanvasRenderer>();
            blank.AddComponent<Slider>();
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(160,20);
            blank.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);

            GameObject background = CreateImage();
            background.name = "Background";
            background.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            background.GetComponent<RectTransform>().anchorMin = new Vector2(0,0.25f); 
            background.GetComponent<RectTransform>().anchorMax = new Vector2(1,0.75f);
            background.GetComponent<Image>().type = Image.Type.Sliced;
            background.GetComponent<Image>().sprite = instance.sprite;
            background.GetComponent<Image>().color = new Color32(200,200,200,255);

            GameObject fillArea = new GameObject();
            fillArea.name = "Fill Area";
            fillArea.AddComponent<RectTransform>();
            fillArea.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            fillArea.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            fillArea.GetComponent<RectTransform>().anchorMin = new Vector2(0,0.25f);
            fillArea.GetComponent<RectTransform>().anchorMax = new Vector2(1,0.75f);
            fillArea.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5,0);
            fillArea.GetComponent<RectTransform>().sizeDelta = new Vector2(-20,0);
            fillArea.GetComponent<RectTransform>().localPosition = new Vector3(-5,0,0);

            GameObject fill = CreateImage();
            fill.name = "Fill";
            fill.AddComponent<RectTransform>();
            fill.AddComponent<CanvasRenderer>();
            fill.GetComponent<RectTransform>().SetParent(fillArea.GetComponent<RectTransform>());
            fill.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            fill.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            fill.GetComponent<RectTransform>().anchorMax = new Vector2(0,1);
            fill.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            fill.GetComponent<RectTransform>().sizeDelta = new Vector2(10,0);
            fill.GetComponent<RectTransform>().localPosition = new Vector3(-70,0,0);

            GameObject handleSlideArea = new GameObject();
            handleSlideArea.name = "Handle Slide Area";
            handleSlideArea.AddComponent<RectTransform>();
            handleSlideArea.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            handleSlideArea.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            handleSlideArea.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            handleSlideArea.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
            handleSlideArea.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            handleSlideArea.GetComponent<RectTransform>().sizeDelta = new Vector2(-20,0);
            handleSlideArea.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);

            GameObject handle = CreateImage();
            handle.name = "Handle";
            handle.AddComponent<RectTransform>();
            handle.AddComponent<CanvasRenderer>();
            handle.AddComponent<Image>();
            handle.GetComponent<RectTransform>().SetParent(handleSlideArea.GetComponent<RectTransform>());
            handle.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            handle.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            handle.GetComponent<RectTransform>().anchorMax = new Vector2(0,1);
            handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            handle.GetComponent<RectTransform>().sizeDelta = new Vector2(20,0);
            handle.GetComponent<RectTransform>().localPosition = new Vector3(-70,0,0);
            
            blank.GetComponent<Slider>().fillRect = fill.GetComponent<RectTransform>();
            blank.GetComponent<Slider>().handleRect = handle.GetComponent<RectTransform>();
            blank.GetComponent<Slider>().targetGraphic = handle.GetComponent<Image>();
            return blank;
        }
        public static GameObject CreateScrollbar()
        {
            GameObject blank = CreateImage();
            blank.name = "Scrollbar";
            blank.AddComponent<Scrollbar>();
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,20);
            blank.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            blank.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);
            blank.GetComponent<Image>().color = new Color32(200,200,200,255);

            GameObject slidingArea = new GameObject();
            slidingArea.name = "Sliding Area";
            slidingArea.AddComponent<RectTransform>();
            slidingArea.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            slidingArea.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            slidingArea.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            slidingArea.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
            slidingArea.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            slidingArea.GetComponent<RectTransform>().sizeDelta = new Vector2(-20,-20);
            slidingArea.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);

            GameObject handle = CreateImage();
            handle.name = "Handle";
            handle.GetComponent<RectTransform>().SetParent(slidingArea.GetComponent<RectTransform>());
            handle.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
            handle.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            handle.GetComponent<RectTransform>().anchorMax = new Vector2(0.05f,1);
            handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            handle.GetComponent<RectTransform>().sizeDelta = new Vector2(20,20);
            handle.GetComponent<RectTransform>().localPosition = new Vector3(-85.5f,0,0);
            
            blank.GetComponent<Scrollbar>().handleRect = handle.GetComponent<RectTransform>();
            blank.GetComponent<Scrollbar>().targetGraphic = handle.GetComponent<Image>();
            return blank;
        }
    }
}
