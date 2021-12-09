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


    //------reactie test
    /*
    public float reactieScore;
    public List<float> reactieTestScores = new List<float>();

    void LoadReactieList()
    {
        // AKG is aantal keren gespeelt
        for (int i = 1; i < PlayerPrefs.GetInt("AKG") + 1; i++)
        {
            reactieScore = PlayerPrefs.GetFloat("SN_" + PlayerPrefs.GetInt("AKG"));
            reactieTestScores.Add(reactieScore);
        }
    }
    */
    //------

    /// <summary>
    /// 
    /// </summary>
    private int savedListCount;
    public int simonScore;

    public List<int> newSimonSaysScores = new List<int>();
    public void LoadNewSimonList()
    {
        savedListCount = PlayerPrefs.GetInt("aantalSimonScores");
        if (savedListCount != 0)
        {
            for (int i = 0; i < savedListCount; i++)
            {
                simonScore = PlayerPrefs.GetInt("simonSaysScores" + i);
                newSimonSaysScores.Add(simonScore);
            }
        }
    }

    private void Awake()
    {
        //LoadReactieList();
        // loading the simon says score list.
        LoadNewSimonList();

        // Pak de base objects referenties
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

        /// <summary>
        /// eerste list is een list voor het testen.
        /// tweede list is een list die niet gebruikt word maar normaal zou je ook de reactieTestScores kunnen laten zien in een grafiek maar IVM weinig tijd en problemen met floats hebben we het eruit gehaalt.
        /// </summary>
        //List<int> valueList = new List<int> {30, 20, 5, 7, 10, 15, 8, 5, 9, 13};
        //List<float> valueList = reactieTestScores;

        // vult de list met newSimonSaysScores (<- ook een list)
        List<int> valueList = newSimonSaysScores;


        lineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.green, new Color(1, 1, 1, .5f));
        barChartVisual = new BarChartVisual(graphContainer, Color.green, .8f);
        ShowGraph(valueList, barChartVisual, -1, (int _i) => ""/*<- Hier kan je iets neerzetten wat voor de getallen op de x as komt*/ +(_i+1), (float _f) => "Score: " + Mathf.RoundToInt(_f));

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

    /// <summary>
    /// ShowGraph laat de grafiek zien door hem te bouwen op basis van de argument values.
    /// </summary>
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


        ///<summary>
        /// zet/berekent de yMaximum en de yMinimum van de grafiek.
        /// </summary>
        // floats voor de yMaximum en de yMinimum
        float yMaximum = valueList[0];
        float yMinimum = valueList[0];

        // for loop die loopt door de valueList heen om te kijken of value groter of kleiner is dan yMaximum en Yminimum zo ja dan veranderd hij de yMaximum of yMinimum
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

        // check of de yDifference kleiner is dan 0 zo ja zet yDifference op 5f.
        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0) yDifference = 5f;

        // crieerd een 20% margin.
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
            lastDot = (i == valueList.Count - maxVisibleValueAmount) ? true : false;// dit is eigenlijk een check om te zorgen of er wel echt een lijn kan geplaatst worden zodat er geen lijn komt tussen het laatste en eerste punt.

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
        int seperatorCount = 5;
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

    /// <summary>
    /// Maakt een interface genaamd IGraphViusal dat een GameObject list crieerd die GraphVisuals toevoegd.
    /// </summary>
    private interface IGraphVisual
    {
        List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth);
    }

    /// <summary>
    /// Maakt een class aan die gebruik maakt van IGraphVisual en de class heet BarcharVisual.
    /// </summary>
    private class BarChartVisual : IGraphVisual
    {
        // RectTransform voor de graphContainer.
        private RectTransform graphContainer;

        // Color customisation.
        private Color barColor;

        // float voor de multiplier word gebruikt voor de barWidth.
        private float barWidthMultiplier;

        /// <summary>
        /// zorgt ervoor dat de visual de correcte container, color en multiplier gebruikt.
        /// </summary>
        public BarChartVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier)
        {
            this.graphContainer = graphContainer;
            this.barColor = barColor;
            this.barWidthMultiplier = barWidthMultiplier;
        }

        /// <summary>
        /// zet de CreateBar op de barGameObject en voegt hem toe aan de List<GameObject>
        /// </summary>
        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth)
        {
            GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth);// gebruikt de graphPosition en de graphPositionWidth om de CreateBar uit te voeren.
            return new List<GameObject>() { barGameObject };
        }

        /// <summary>
        /// maakt de bar aan met de bijbehoorende posities en barWidth.
        /// Ook wordt hier het uiterlijk van de bar aangemaakt.
        /// </summary>
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

    /// <summary>
    /// Hier wordt een class gemaakt met de IGraphVisual genaamd LineGraphVisual.
    /// </summary>
    private class LineGraphVisual : IGraphVisual
    {
        // benodigde privates voor de costumisation.
        private RectTransform graphContainer;
        private Sprite dotSprite;
        private GameObject lastDotGameObject;
        private Color dotColor;
        private Color dotConnectionColor;

        /// <summary>
        /// Zorgt ervoor dat de visual de correcte container, color, dotConnectionColor en multiplier gebruikt.
        /// </summary>
        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotConnectionColor)
        {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColor = dotColor;
            this.dotConnectionColor = dotConnectionColor;
            lastDotGameObject = null;
        }

        /// <summary>
        /// Hier worden de punten bekeken en er word gecheckt of er een lijn kan gemaakt worden.
        /// </summary>
        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth)
        {
            List<GameObject> gameObjectList = new List<GameObject>();
            GameObject dotGameObject = CreateDot(graphPosition);
            gameObjectList.Add(dotGameObject);
            // check of de laatste dot game object niet null is en of de bool lastDot op false staat.
            if (lastDotGameObject != null && lastDot == false)
            {
                // crieerd de dot connection.
                GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastDotGameObject = dotGameObject;
            return gameObjectList;
        }

        /// <summary>
        /// Maakt een dot aan met de goede bij behoorende positie.
        /// Ook wordt er hier gebruik gemaakt van costumisation.
        /// </summary>
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

        /// <summary>
        /// Hier word de dot connection gemaakt.
        /// Er worder een paar berekeningen gedaan voor het goet plaatsen van de lines van de linerenderer.
        /// Ook word er gebruik gemaakt van de eerder aangegeven costumisations.
        /// </summary>
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

    /// <summary>
    /// Hieronder de buttons voor de functies die eerder zijn gemaakt.
    /// </summary>
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