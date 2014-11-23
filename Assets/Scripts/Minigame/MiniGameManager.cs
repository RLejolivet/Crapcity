using UnityEngine;
using System.Collections;

public class MiniGameManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] cartes = new GameObject[8];
    private Texture[] OriginalTexture = new Texture[8];

    [SerializeField]
    private Texture versoCarte;

    private int cooldown = 0;
    private int nb_cartes;
    private int visibleCard = -1;
    private int visibleCard2 = -1;
    private int nbPaires = 0;

    private void Shuffler()
    {
        System.Random r = new System.Random();
        int nbRandom = 100;
        int r1;
        int r2;
        Vector3 aux = new Vector3();
        while (nbRandom > 0)
        {
            r1 = (int)(r.Next() / (float)int.MaxValue * 8);
            r2 = (int)(r.Next() / (float)int.MaxValue * 8);


            aux = (cartes[r1]).transform.position;
            (cartes[r1]).transform.position = (cartes[r2]).transform.position;
            (cartes[r2]).transform.position = aux;

            nbRandom--;
        }
    }

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 8; i++)
        {
            OriginalTexture[i] = (cartes[i]).GetComponent<GUITexture>().texture;
            cartes[i].GetComponent<GUITexture>().texture = versoCarte;
        }
        Shuffler();
	}

    // Update is called once per frame
    void Update()
    {
        if (cooldown % 60 == 0)
        {
            int ij = isAnyTextureClicked();
            switch (nb_cartes)
            {
                case 0:
                    if (ij >= 0 && ij != visibleCard)
                    {
                        nb_cartes++;
                        visibleCard = ij;
                        cartes[ij].GetComponent<GUITexture>().texture = OriginalTexture[ij];
                    }
                    break;

                case 1:
                    if (ij >= 0 && ij != visibleCard)
                    {
                        cartes[ij].GetComponent<GUITexture>().texture = OriginalTexture[ij];
                        if ((ij == 0 && visibleCard == 1) || (ij == 1 && visibleCard == 0) || (ij == 2 && visibleCard == 3) || (ij == 3 && visibleCard == 2)
                            || (ij == 4 && visibleCard == 5) || (ij == 5 && visibleCard == 4) || (ij == 6 && visibleCard == 7) || (ij == 7 && visibleCard == 6))
                        {
                            nbPaires++;
                            visibleCard = -1;
                            nb_cartes = 0;
                        }
                        else
                        {
                            cooldown = 59;
                            visibleCard2 = ij;
                            nb_cartes = 0;
                        }
                    }
                    break;
            }
        }

        //if (cooldown != 0) Debug.Log(cooldown);


        if (nbPaires == 4 && cooldown % 60 == 0)
        {
            cooldown = 119;
        }
        if (nbPaires == 4 && cooldown % 61 == 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        if (cooldown == 1)
        {
            cartes[visibleCard].GetComponent<GUITexture>().texture = versoCarte;
            cartes[visibleCard2].GetComponent<GUITexture>().texture = versoCarte;
            visibleCard = -1;
            visibleCard2 = -1;
        }

        cooldown = (cooldown > 0) ? cooldown - 1 : cooldown;
	}

    private int isAnyTextureClicked()
    {
        for (int i = 0; i < 8; i++)
        {
            if (isGuiTextureClicked(cartes[i].GetComponent<GUITexture>())) return i;
        }
        return -1;
    }

    private bool isGuiTextureClicked(GUITexture tex)
    {
        if (tex.HitTest(Input.mousePosition))
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
