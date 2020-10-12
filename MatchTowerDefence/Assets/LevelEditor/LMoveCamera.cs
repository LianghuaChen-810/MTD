using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class LMoveCamera : MonoBehaviour
    {
        public float movespeed=0.1f;
        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(new Vector3(0, 1f * movespeed, 0));
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(new Vector3(0, -1f * movespeed, 0));
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(new Vector3(-1f * movespeed, 0, 0));
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(new Vector3(1f * movespeed, 0, 0));
            }
        }
    }
}
