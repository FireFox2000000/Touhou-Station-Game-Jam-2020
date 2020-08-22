using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class UICredits : FrontendMenuBase
{
    [SerializeField]
    TextMeshProUGUI creditsText;

    void Start()
    {
        // Populate credits text manually to skip localisation

        string creditsStr = Localiser.Instance.GetLocalised("ViewCredits");
        string jennRoleStr = Localiser.Instance.GetLocalised("ProductionAndGameplayProgramming");
        string firefoxRoleStr = Localiser.Instance.GetLocalised("EngineFrontendSystemProgramming");
        string nnekonRoleStr = Localiser.Instance.GetLocalised("CharacterArtAnimation");
        string kurantoRoleStr = Localiser.Instance.GetLocalised("BackgroundArtTranslation");
        string raeRoleStrStr = Localiser.Instance.GetLocalised("MusicDialogue");
        string testersStr = Localiser.Instance.GetLocalised("Testers");
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("<b>{0}</b>\n\n", creditsStr);
        sb.AppendFormat("<align=\"left\">Jenn Raye <indent=15%>- {0}</indent>\n\n", jennRoleStr);
        sb.AppendFormat("FireFox <indent=15%>- {0}</indent>\n\n", firefoxRoleStr);
        sb.AppendFormat("Nekon <indent=15%>- {0}</indent>\n\n", nnekonRoleStr);
        sb.AppendFormat("KurantoB <indent=15%>- {0}</indent>\n\n", kurantoRoleStr);
        sb.AppendFormat("RaeRae <indent=15%>- {0}</indent>\n\n", raeRoleStrStr);
        sb.AppendFormat("</align>", raeRoleStrStr);
        sb.AppendFormat("{0}\n", testersStr);
        sb.AppendFormat("Aly Doglesbian\n");
        sb.AppendFormat("Emma Bonne\n");
        creditsText.text = sb.ToString();
    }
}
