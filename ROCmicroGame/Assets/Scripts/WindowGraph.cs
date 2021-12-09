/*

Door: Léon Conner Blaauw

In de toekomst zou ik graag de bug willen fixen waar als er teveel punten zijn in de grafiek,
er de labels op de x as te dicht op elkaar gaan staan.

Dit zou ik hoogst waarschijnlijk kunnen fixen door te kijken naar de .count van de list
en als de list een x aantal aan variabelen erin heeft, de x as labels een x aantal keren over te slaan per keer na dat een x as label is geplaatst.

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    IGraphVisual lineGraphVisual;
    IGraphVisual barChartVisual;

    public static bool lastDot;

    //
    [SerializeField] private Sprite dotSprite;

    private RectTransform graphContainer;

    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;

    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    private List<GameObject> gameObjectList;

    // Cached Values
    private List<int> valueList;
    private IGraphVisual graphVisual;
    private int maxVisibleValueAmount;
    private Func<int, string> getAxisLabelX;
    private Func<float, string> getAxisLabelY;

    //------------
    private int savedListCount;
    public int score;

    public List<int> newSimonSaysScores = new List<int>();
    public void LoadNewSimonList()
    {
        PlayerPrefs.SetInt("aantalSimonScores", PlayerPrefs.GetInt("aantalSimonScores") + 1);
        savedListCount = PlayerPrefs.GetInt("aantalSimonScores");

        for (int i = 0; i < savedListCount; i++)
        {
            score = PlayerPrefs.GetInt("simonSaysScores" + i);
            newSimonSaysScores.Add(score);
        }
    }

    //------------

    private void Awake()
    {
        LoadNewSimonList();
        // Pak de base objects referenties
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

        List<int> valueList = newSimonSaysScores;
        lineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.green, new Color(1, 1, 1, .5f));
        barChartVisual = new BarChartVisual(graphContainer, Color.green, .8f);
        ShowGraph(valueList, barChartVisual, -1, (int _i) => ""/*<- Hier kan je iets neerzetten wat voor de getallen op de x as komt*/ +(_i+1), (float _f) => "$ " + Mathf.RoundToInt(_f));

        /*bool useBarChart = false;
        if (useBarChart)
        {
            ShowGraph(valueList, barChartVisual, -1, (int _i) => "Day" + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));
        }
        else
        {
            ShowGraph(valueList, lineGraphVisual, -1, (int _i) => "Day" + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));
        }
        */
    }

    private void SetGraphVisual(IGraphVisual graphVisual)
    {
        ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);
    }

    private void ShowGraph(List<int> valueList, IGraphVisual graphVisual, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        this.valueList = valueList;
        this.graphVisual = graphVisual;
        this.getAxisLabelX = getAxisLabelX;
        this.getAxisLabelY = getAxisLabelY;

        if (maxVisibleValueAmount <= 0)
        {
            // Show all if no amount specified.
            maxVisibleValueAmount = valueList.Count;
        }

        if (maxVisibleValueAmount > valueList.Count)
        {
            // valideer de amount om de maximum weer te geven
            // maxVisibleValueAmount = valueList.Count;
            maxVisibleValueAmount = 1;
        }

        this.maxVisibleValueAmount = maxVisibleValueAmount;

        // Test voor label defaults
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i)// delegate word gebruikt om methods te passen als argumments naar andere methods.
            {
                return _i.ToString();
            };
        }

        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f)
            {
                return Mathf.RoundToInt(_f).ToString();
            };
        }


        // Maakt de vorige grafiek schoon.
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        // pakt de breedte en de hoogte van de container.
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMaximum = valueList[0];
        float yMinimum = valueList[0];

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            int value = valueList[i];
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0) yDifference = 5f;

        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        yMinimum = 0f; // Dit zorgt ervoor dat de grafiek op 0 begint.

        float xSize = graphWidth / (maxVisibleValueAmount + 1);

        // Loop alle visuele data punten door.
        int xIndex = 0;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            /*
            // lastDot = (i == valueList.Count - maxVisibleValueAmount) ? true : false; doet het zelfde als:
            if (i == valueList.Count - maxVisibleValueAmount)
            {
                lastDot = true;
            }
            else
            {
                lastDot = false;
            }

            //of dit zou ook in mijn geval werken.

            lastDot = false;
            if (i == valueList.Count - maxVisibleValueAmount) lastDot = true;
            ____________________________________________________

            //valueList.Count - maxVisibleValueAmount zorgt ervoor dat het programma mee telt of dit het eerste punt is van de zichtbare punten in de grafiek.

            */
            lastDot = (i == valueList.Count - maxVisibleValueAmount) ? true : false;

            gameObjectList.AddRange(graphVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize));

            // Dupliceer de x label template.
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            // Dupliceer de x dash template.
            RectTransform dashX = Instantiate(dashTemplateY);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -3f);
            gameObjectList.Add(dashX.gameObject);

            xIndex++;
        }
        
        // Set up seperators op de y axis
        int seperatorCount = 10;
        for (int i = 0; i <= seperatorCount; i++)
        {
            // Dupliceer de y label template.
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / seperatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            gameObjectList.Add(labelY.gameObject);

            // Dupliceer the y dash template.
            RectTransform dashY = Instantiate(dashTemplateX);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            gameObjectList.Add(dashY.gameObject);
        }
    }

    private interface IGraphVisual
    {
        List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth);
    }

    private class BarChartVisual : IGraphVisual
    {
        private RectTransform graphContainer;
        private Color barColor;
        private float barWidthMultiplier;

        public BarChartVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier)
        {
            this.graphContainer = graphContainer;
            this.barColor = barColor;
            this.barWidthMultiplier = barWidthMultiplier;
        }

        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth)
        {
            GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth);
            return new List<GameObject>() { barGameObject };
        }

        private GameObject CreateBar(Vector2 graphPosition, float barWidth)
        {
            GameObject gameObject = new GameObject("bar", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = barColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
            rectTransform.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(.5f, 0f);
            return gameObject;
        }
    }

    private class LineGraphVisual : IGraphVisual
    {
        private RectTransform graphContainer;

        private Sprite dotSprite;

        private GameObject lastDotGameObject;

        private Color dotColor;

        private Color dotConnectionColor;

        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotConnectionColor)
        {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColor = dotColor;
            this.dotConnectionColor = dotConnectionColor;
            lastDotGameObject = null;
        }

        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth)
        {
            List<GameObject> gameObjectList = new List<GameObject>();
            GameObject dotGameObject = CreateDot(graphPosition);
            gameObjectList.Add(dotGameObject);
            if (lastDotGameObject != null && lastDot == false)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastDotGameObject = dotGameObject;
            return gameObjectList;
        }

        GameObject gameObject;

        private GameObject CreateDot(Vector2 anchoredPosition)
        {
            gameObject = new GameObject("dot", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().sprite = dotSprite;
            gameObject.GetComponent<Image>().color = dotColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            return gameObject;
        }

        private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
        {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = dotConnectionColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, (Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI));

            return gameObject;
        }
    }

    public void BarChartBtn()
    {
        SetGraphVisual(barChartVisual);
    }

    public void LineGraphBtn()
    {
        SetGraphVisual(lineGraphVisual);
    }

    public void decreaseVisibleAmountBtn()
    {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount - 1, this.getAxisLabelX, this.getAxisLabelY);
    }
    public void increaseVisibleAmountBtn()
    {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount + 1, this.getAxisLabelX, this.getAxisLabelY);
    }
}