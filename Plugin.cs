using BepInEx;
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

namespace UKUIHelper
{
    [BepInPlugin("zed.uk.uihelper", "UK UI Helper", "0.6.2")]
    public class UIHelper : BaseUnityPlugin
    {
        private Sprite sprite,checkMarkSprite,dropDownSprite;
        private Font font;
        
        private static UIHelper instance;

        private string path,workDir;
        private void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += Scene;
            instance = this;
            path = System.Reflection.Assembly.GetAssembly(typeof(UIHelper)).Location;
            workDir = Path.GetDirectoryName(path);
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll",SearchOption.AllDirectories);
            Logger.LogInfo("Loading UI ressources");
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            Texture2D texture1 = new Texture2D(200, 200);
            Texture2D texture2 = new Texture2D(64, 64);
            Texture2D texture3 = new Texture2D(64, 64);
            texture1.LoadImage(File.ReadAllBytes(workDir + "\\Sprites\\Sprite.png")); 
            sprite = Sprite.Create(texture1, new Rect(0, 0, 200, 200), new Vector2(0.5f, 0.5f),200f,0,SpriteMeshType.Tight,new Vector4(15,15,15,15));
            texture2.LoadImage(File.ReadAllBytes(workDir + "\\Sprites\\Checkmark.png")); 
            checkMarkSprite = Sprite.Create(texture2, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f),64f,0,SpriteMeshType.Tight);
            texture3.LoadImage(File.ReadAllBytes(workDir + "\\Sprites\\Dropdown.png")); 
            dropDownSprite = Sprite.Create(texture3, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f),64f,0,SpriteMeshType.Tight);
            Logger.LogInfo("Done");
        }
        void Scene(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
        {
            if(scene.name.Contains("Menu"))
            {
                Logger.LogInfo("Main scene loaded");
            }
        }
        /// <summary>
        /// Loads a sprite from a file
        /// </summary>
        public static Sprite LoadSprite(string path,Vector4 border,float pixelsPerUnit)
        {
            Texture2D texture = new Texture2D(200, 200);
            texture.LoadImage(File.ReadAllBytes(path));
            return Sprite.Create(texture, new Rect(0, 0,texture.width,texture.height),new Vector2(0.5f, 0.5f),pixelsPerUnit,0,SpriteMeshType.Tight,border);
        }
        public static GameObject CreateButton()
        {
            GameObject blank = new GameObject();
            blank.name = "Button";
            blank.AddComponent<RectTransform>();
            blank.AddComponent<CanvasRenderer>();
            blank.AddComponent<Image>();
            blank.AddComponent<Button>();
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,50);
            blank.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
            blank.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
            blank.GetComponent<Image>().sprite = instance.sprite;
            blank.GetComponent<Image>().type = Image.Type.Sliced;
            blank.GetComponent<Button>().targetGraphic = blank.GetComponent<Image>();

            GameObject text = CreateText();
            text.name = "Text";
            text.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            text.GetComponent<RectTransform>().SetAnchor(AnchorPresets.StretchAll);
            text.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
            text.GetComponent<Text>().text = "Button";
            text.GetComponent<Text>().fontSize = 32;
            text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            text.GetComponent<Text>().color = Color.black;
            return blank;
        }
        public static GameObject CreateText()
        {
            GameObject blank = new GameObject();
            blank.name = "Text";
            blank.AddComponent<RectTransform>();
            blank.AddComponent<CanvasRenderer>();
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,50);
            blank.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
            blank.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
            blank.AddComponent<Text>();
            blank.GetComponent<Text>().text = "Text";
            blank.GetComponent<Text>().font = instance.font;
            blank.GetComponent<Text>().fontSize = 32;
            blank.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            blank.GetComponent<Text>().color = Color.black;
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
            return blank;
        }*/
        public static GameObject CreateInputField()
        {
            GameObject blank = CreateImage();
            blank.name = "InputField";
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,40);
            blank.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
            blank.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
            
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
            text.GetComponent<RectTransform>().SetAnchor(AnchorPresets.StretchAll);
            text.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(-20,0);

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
            return blank;
        }
        public static GameObject CreatePanel()
        {
            GameObject blank = CreateImage();
            blank.name = "Panel";
            blank.GetComponent<RectTransform>().sizeDelta = new Vector2(200,200);
            blank.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
            blank.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
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
            blank.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
            blank.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
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
            blank.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
            blank.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);

            GameObject background = CreateImage();
            background.name = "Background";
            background.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            background.GetComponent<RectTransform>().sizeDelta = new Vector2(30,30);
            background.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
            background.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);

            GameObject checkmark = CreateImage();
            checkmark.name = "Checkmark";
            checkmark.GetComponent<RectTransform>().SetParent(background.GetComponent<RectTransform>());
            checkmark.GetComponent<RectTransform>().SetAnchor(AnchorPresets.StretchAll);
            checkmark.GetComponent<RectTransform>().offsetMin = new Vector2(5,5);
            checkmark.GetComponent<RectTransform>().offsetMax = new Vector2(-5,-5);
            checkmark.GetComponent<Image>().color = Color.black;
            checkmark.GetComponent<Image>().sprite = instance.checkMarkSprite;

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
            blank.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
            blank.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);

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
            fillArea.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
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
            fill.GetComponent<RectTransform>().SetAnchor(AnchorPresets.VertStretchLeft);
            fill.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
            fill.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            fill.GetComponent<RectTransform>().sizeDelta = new Vector2(10,0);
            fill.GetComponent<RectTransform>().localPosition = new Vector3(-70,0,0);

            GameObject handleSlideArea = new GameObject();
            handleSlideArea.name = "Handle Slide Area";
            handleSlideArea.AddComponent<RectTransform>();
            handleSlideArea.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            handleSlideArea.GetComponent<RectTransform>().SetAnchor(AnchorPresets.StretchAll);
            handleSlideArea.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
            handleSlideArea.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            handleSlideArea.GetComponent<RectTransform>().sizeDelta = new Vector2(-20,0);
            handleSlideArea.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);

            GameObject handle = CreateImage();
            handle.name = "Handle";
            handle.AddComponent<RectTransform>();
            handle.AddComponent<CanvasRenderer>();
            handle.AddComponent<Image>();
            handle.GetComponent<RectTransform>().SetParent(handleSlideArea.GetComponent<RectTransform>());
            handle.GetComponent<RectTransform>().SetAnchor(AnchorPresets.VertStretchLeft);
            handle.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
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
            blank.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
            blank.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
            blank.GetComponent<Image>().color = new Color32(200,200,200,255);

            GameObject slidingArea = new GameObject();
            slidingArea.name = "Sliding Area";
            slidingArea.AddComponent<RectTransform>();
            slidingArea.GetComponent<RectTransform>().SetParent(blank.GetComponent<RectTransform>());
            slidingArea.GetComponent<RectTransform>().SetAnchor(AnchorPresets.StretchAll);
            slidingArea.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
            slidingArea.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            slidingArea.GetComponent<RectTransform>().sizeDelta = new Vector2(-20,-20);
            slidingArea.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);

            GameObject handle = CreateImage();
            handle.name = "Handle";
            handle.GetComponent<RectTransform>().SetParent(slidingArea.GetComponent<RectTransform>());
            handle.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
            handle.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            handle.GetComponent<RectTransform>().anchorMax = new Vector2(0.05f,1);
            handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            handle.GetComponent<RectTransform>().sizeDelta = new Vector2(20,20);
            handle.GetComponent<RectTransform>().localPosition = new Vector3(-85.5f,0,0);
            
            blank.GetComponent<Scrollbar>().handleRect = handle.GetComponent<RectTransform>();
            blank.GetComponent<Scrollbar>().targetGraphic = handle.GetComponent<Image>();
            return blank;
        }
        public static GameObject CreateOverlay(bool persistent = false)
        {
            GameObject blank = new GameObject();
            blank.name = "Overlay";
            blank.AddComponent<Canvas>();
            blank.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            blank.GetComponent<Canvas>().sortingOrder = 1000;
            blank.AddComponent<CanvasScaler>();
            blank.AddComponent<GraphicRaycaster>();
            blank.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            blank.GetComponent<CanvasScaler>().matchWidthOrHeight = 0f;
            blank.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920,1080);
            if(persistent)
            {
                DontDestroyOnLoad(blank);
            }
            return blank;
        }
    }
}
