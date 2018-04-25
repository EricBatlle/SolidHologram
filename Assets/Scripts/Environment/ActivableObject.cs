using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class ActivableObject : MonoBehaviour {

    #region setPlayersReference variables
    protected LineDraw_Net bentley;
    protected PlatformerCharacter2D box;

    protected bool findBoxReference = false;
    protected bool findBentleyReference = false;
    protected bool playersReference = true;

    public event Action OnBoxReferenceFinded = null;
    public event Action OnBentleyReferenceFinded = null;    
    #endregion

    public virtual void StartBehaviour()
    {
    }
    public virtual void EndBehaviour()
    {
    }

    //Used to set players reference
    private void FixedUpdate()
    {
        //Set Players References       
        if (!findBoxReference)
        {
            GameObject boxGO = GameObject.FindGameObjectWithTag("Player");
            if (boxGO != null)
            {
                box = boxGO.GetComponent<PlatformerCharacter2D>();
                findBoxReference = true;

                if (OnBoxReferenceFinded != null)
                    OnBoxReferenceFinded();
            }
        }
        if (!findBentleyReference)
        {
            GameObject bentleyGO = GameObject.FindGameObjectWithTag("Bentley");
            if (bentleyGO != null)
            {
                bentley = bentleyGO.GetComponent<LineDraw_Net>();
                findBentleyReference = true;

                if(OnBentleyReferenceFinded != null)
                    OnBentleyReferenceFinded();
            }
        }

        playersReference = (findBoxReference && findBentleyReference) ? true : false;       
    }
}
