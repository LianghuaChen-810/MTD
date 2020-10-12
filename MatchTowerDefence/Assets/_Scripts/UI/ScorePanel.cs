﻿using UnityEngine;
using UnityEngine.UI;

namespace MatchTowerDefence.UI
{
    public class ScorePanel : MonoBehaviour
    {
        [SerializeField] private Image[] starImages;
        [SerializeField] private Sprite achievedStarSprite;

		public void SetStars(int score)
		{
			if (score <= 0) {	return;	}

			for (int i = 0; i < score; i++)
			{
				starImages[i].sprite = achievedStarSprite;
			}
		}
	}
}