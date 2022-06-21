using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int GameScore = 10;
    public GameObject TopRow;
    public GameObject BottomRow;
    public TextMeshProUGUI GameScoreText;
    public Button PlayButton;
    public List<GameObject> Fruits;
    public int FruitSize;
    public int StartingPos;
    public int EndingPos;
    public float TimeToMoveFruits;
    private List<RectTransform> _topRowFruits;
    private List<RectTransform> _bottomRowFruits;

    private RectTransform GenerateFruit(int x, GameObject parent)
    {
        int fruitNumber = Random.Range(0, Fruits.Count);
        GameObject fruit = Instantiate(Fruits[fruitNumber]);
        RectTransform fruitRect = fruit.GetComponent<RectTransform>();
        fruit.transform.SetParent(parent.transform);
        fruitRect.anchoredPosition = new Vector2(x, 0);
        fruitRect.sizeDelta = new Vector2(FruitSize, FruitSize);
        fruitRect.localScale = new Vector3(1, 1, 1);
        return fruitRect;
    }
    private void GenerateField()
    {
        for (int i = 0; i < 4; i++)
        {
            _topRowFruits.Add(GenerateFruit((StartingPos + FruitSize * i), TopRow));
            _bottomRowFruits.Add(GenerateFruit((StartingPos + FruitSize * i), BottomRow));
        }
    }
    private void AddScrore()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_topRowFruits[i].gameObject.GetComponent<Image>().sprite == _bottomRowFruits[i].gameObject.GetComponent<Image>().sprite)
            {
                GameScore *= 2;
                GameScoreText.text = GameScore.ToString();
            }
        }
    }
    private void Play()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCards());
    }
    IEnumerator MoveCards()
    {
        PlayButton.enabled = false;
        int numberOfFruitsToMove = Random.Range(4, 11);
        int moveRightTop = Random.Range(0, 2);
        int moveRightBottom = Random.Range(0, 2);
        for (int i = 1; i <= numberOfFruitsToMove; i++)
        {
            if (moveRightTop == 0)
            {
                _topRowFruits.Insert(0, GenerateFruit((StartingPos - FruitSize * i), TopRow));
            }
            else if (moveRightTop == 1)
            {
                _topRowFruits.Add(GenerateFruit((EndingPos + FruitSize * i), TopRow));
            }
            if (moveRightBottom == 0)
            {
                _bottomRowFruits.Insert(0, GenerateFruit((StartingPos - FruitSize * i), BottomRow));
            }
            else if (moveRightBottom == 1)
            {
                _bottomRowFruits.Add(GenerateFruit((EndingPos + FruitSize * i), BottomRow));
            }
        }
        for (int j = 0; j < numberOfFruitsToMove; j++)
        {
            float counter = 0f;
            List<Vector3> startPosTop = new List<Vector3>();
            List<Vector3> startPosBottom = new List<Vector3>();
            for (int i = 0; i < _topRowFruits.Count; i++)
            {
                startPosTop.Add(_topRowFruits[i].anchoredPosition);
            }
            for (int i = 0; i < _bottomRowFruits.Count; i++)
            {
                startPosBottom.Add(_bottomRowFruits[i].anchoredPosition);
            }
            while (counter < TimeToMoveFruits)
            {
                counter += Time.deltaTime;
                for (int i = 0; i < _topRowFruits.Count; i++)
                {
                    if (moveRightTop == 0)
                    {
                        _topRowFruits[i].anchoredPosition = Vector3.Lerp(startPosTop[i], startPosTop[i] + new Vector3(FruitSize, 0, 0), counter / TimeToMoveFruits);
                    }
                    else if (moveRightTop == 1)
                    {
                        _topRowFruits[i].anchoredPosition = Vector3.Lerp(startPosTop[i], startPosTop[i] - new Vector3(FruitSize, 0, 0), counter / TimeToMoveFruits);
                    }
                }
                for (int i = 0; i < _bottomRowFruits.Count; i++)
                {
                    if (moveRightBottom == 0)
                    {
                        _bottomRowFruits[i].anchoredPosition = Vector3.Lerp(startPosBottom[i], startPosBottom[i] + new Vector3(FruitSize, 0, 0), counter / TimeToMoveFruits);
                    }
                    else if (moveRightBottom == 1)
                    {
                        _bottomRowFruits[i].anchoredPosition = Vector3.Lerp(startPosBottom[i], startPosBottom[i] - new Vector3(FruitSize, 0, 0), counter / TimeToMoveFruits);
                    }
                }
                yield return null;
            }
        }
        for (int i = 0; i < numberOfFruitsToMove; i++)
        {
            if (moveRightBottom == 0 && _bottomRowFruits[_bottomRowFruits.Count - 1].anchoredPosition.x > EndingPos)
            {
                Destroy(_bottomRowFruits[_bottomRowFruits.Count - 1].gameObject);
                _bottomRowFruits.RemoveAt(_bottomRowFruits.Count - 1);
            }
            else if (moveRightBottom == 1 && _bottomRowFruits[0].anchoredPosition.x < StartingPos)
            {
                Destroy(_bottomRowFruits[0].gameObject);
                _bottomRowFruits.RemoveAt(0);
            }
            if (moveRightTop == 0 && _topRowFruits[_topRowFruits.Count - 1].anchoredPosition.x > EndingPos)
            {
                Destroy(_topRowFruits[_topRowFruits.Count - 1].gameObject);
                _topRowFruits.RemoveAt(_topRowFruits.Count - 1);
            }
            else if (moveRightTop == 1 && _topRowFruits[0].anchoredPosition.x < StartingPos)
            {
                Destroy(_topRowFruits[0].gameObject);
                _topRowFruits.RemoveAt(0);
            }
        }
        PlayButton.enabled = true;
        AddScrore();
    }
    private void Awake()
    {
        _topRowFruits = new List<RectTransform>();
        _bottomRowFruits = new List<RectTransform>();
        GenerateField();
        PlayButton.onClick.AddListener(Play);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
