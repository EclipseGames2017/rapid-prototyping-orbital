using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class FeedbackButton : MonoBehaviour
    {
        public void OpenFeedbackForm()
        {
            Application.OpenURL("https://goo.gl/forms/JrTFjfilNNWTngoP2");
        }
    }
}