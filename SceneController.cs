using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	public const int gridRows = 2;
	public const int gridCols = 4;
	public const float offsetX = 1f;
	public const float offsetY = 1.5f;
	public int _score = 0;
	private MemoryCard _firstRevealed;
	private MemoryCard _secondRevealed;

	[SerializeField] private MemoryCard originalCard;
	[SerializeField] private Sprite[] images;
	[SerializeField] private TextMesh scoreLabel;

	public bool canReveal {
		get { return _secondRevealed == null; }
	}

	void Start () {
		Vector3 starPos = originalCard.transform.position;

		int[] numbers = {0, 0, 1, 1, 2, 2, 3, 3};

		numbers = ShuffleArray(numbers);

		for (int i = 0; i < gridCols; i++) {
			for(int j = 0; j < gridRows; j++) {
				MemoryCard card;
				if(i==0 && j == 0) {
					card = originalCard;
				} else {
					card = Instantiate(originalCard) as MemoryCard;
				}

				int index = j * gridCols + i;
				int id = numbers[index];
				card.SetCard(id, images[id]);

				float posX = (offsetX * i) + starPos.x;
				float posY = -(offsetX * j) + starPos.y;
				card.transform.position = new Vector3(posX, posY, starPos.z);
			}
		}	
	}

	private int[] ShuffleArray(int[] numbers) {
		int[] newArray = numbers.Clone() as int[];
		for (int i = 0; i < newArray.Length; i++) {
			int tmp = newArray[i];
			int r = UnityEngine.Random.Range(i, newArray.Length);
			newArray[i] = newArray[r];
			newArray[r] = tmp;
		}
		return newArray;
	}

	public void CardRevealed(MemoryCard card) {
		if (_firstRevealed == null) {
			_firstRevealed = card;
		} else {
			_secondRevealed = card;
			StartCoroutine(ChechMath());
		}
	}

	private IEnumerator ChechMath() {
		if(_firstRevealed.id == _secondRevealed.id) {
			_score++;
			scoreLabel.text = "Score: " + _score;
		} else {
			yield return new WaitForSeconds(.5f);
			_firstRevealed.Unreveal();
			_secondRevealed.Unreveal();
		}

		_firstRevealed = null;
		_secondRevealed = null;
	}

	public void Restart() {
		SceneManager.LoadScene("Scene");
	}
}
