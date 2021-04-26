using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@auth: Jared Freeman
//Idea: Instantiate these text bubbles to present strings in 3D space rather than on the HUD
//Motivation: Damage numbers, Resource income indicator, Status and info numbers, etc

public class Text_Bubble : MonoBehaviour
{
    #region MEMBERS
    public static float text_move_speed = 3f;
    public static float text_font_default_size = 20f;
    private string display_message;
    //private TextMesh text_mesh_pro;
    private TMPro.TextMeshPro text_mesh_pro;
    private static Color default_color = Color.white;
    #endregion
    #region EVENTS
    public static event System.EventHandler<MonobehaviourEventArgs> RequestAlignToCameraAnglesEvent;
    #endregion
    #region EVENT SUBSCRIPTIONS
    #endregion
    #region EVENT HANDLERS
    #endregion
    #region INIT
    private void Awake()
    {
        if(text_mesh_pro == null)
            text_mesh_pro = gameObject.AddComponent<TMPro.TextMeshPro>();
        text_mesh_pro.color = default_color;
        text_mesh_pro.fontSize = text_font_default_size;
        text_mesh_pro.alignment = TMPro.TextAlignmentOptions.Midline;

        RequestAlignToCameraAnglesEvent?.Invoke(this, new MonobehaviourEventArgs(this));
    }
    #endregion

    #region CreateTemporaryTextBubble OVERLOADS
    //Helper Method(s):
    private static Text_Bubble CreateTemporaryTextBubble(string message, float duration)
    {
        GameObject goj = new GameObject("Temporary Text Bubble");
        Text_Bubble txt = goj.AddComponent<Text_Bubble>();
        txt.RemoveAfterSeconds(duration);
        txt.UpdateTextMessage(message);
        return txt;
    }
    //Public Methods:
    public static Text_Bubble CreateTemporaryTextBubble(string message, float duration, GameObject parent)
    {
        Text_Bubble txt = CreateTemporaryTextBubble(message, duration);
        txt.gameObject.transform.SetParent(parent.transform);
        return txt;
    }
    public static Text_Bubble CreateTemporaryTextBubble(string message, float duration, GameObject parent, Color color)
    {
        Text_Bubble txt = CreateTemporaryTextBubble(message, duration, parent);
        txt.text_mesh_pro.color = color;
        return txt;
    }
    public static Text_Bubble CreateTemporaryTextBubble(string message, float duration, Vector3 position)
    {
        Text_Bubble txt = CreateTemporaryTextBubble(message, duration);
        txt.gameObject.transform.position = position;
        return txt;
    }
    public static Text_Bubble CreateTemporaryTextBubble(string message, float duration, Vector3 position, Color color)
    {
        Text_Bubble txt = CreateTemporaryTextBubble(message, duration, position);
        txt.text_mesh_pro.color = color;
        return txt;
    }
    #endregion

    #region CreateTextBubble OVERLOADS
    private static Text_Bubble CreateTextBubble(string message)
    {
        GameObject goj = new GameObject("Text Bubble");
        Text_Bubble txt = goj.AddComponent<Text_Bubble>();
        txt.UpdateTextMessage(message);
        return txt;
    }
    public static Text_Bubble CreateTextBubble(string message, GameObject parent)
    {
        Text_Bubble txt = CreateTextBubble(message);
        txt.gameObject.transform.SetParent(parent.transform);
        return txt;
    }
    public static Text_Bubble CreateTextBubble(string message, GameObject parent, Color color)
    {
        Text_Bubble txt = CreateTextBubble(message, parent);
        txt.text_mesh_pro.color = color;
        return txt;
    }
    public static Text_Bubble CreateTextBubble(string message, Vector3 position)
    {
        Text_Bubble txt = CreateTextBubble(message);
        txt.gameObject.transform.position = position;
        return txt;
    }
    public static Text_Bubble CreateTextBubble(string message, Vector3 position, Color color)
    {
        Text_Bubble txt = CreateTextBubble(message, position);
        txt.text_mesh_pro.color = color;
        return txt;
    }
    #endregion

    void UpdateTextMessage(string message)
    {
        display_message = message;
        text_mesh_pro.text = display_message;
    }

    void RemoveAfterSeconds(float duration)
    {
        StartCoroutine(ContinueRemoveAfterSeconds(duration));
    }

    void KillBubble()
    {
        Destroy(gameObject);
    }

    IEnumerator ContinueRemoveAfterSeconds(float duration)
    {
        float start_time = Time.time;
        float cur_time = Time.time;

        while(Mathf.Abs((cur_time - start_time)) < duration)
        {
            gameObject.transform.position += Vector3.up * text_move_speed * Time.deltaTime;
            cur_time = Time.time;
            yield return null;
        }

        KillBubble();
    }
}
