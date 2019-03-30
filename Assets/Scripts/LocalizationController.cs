using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InfinityEngine.Localization;
using UnityEngine;

public class LocalizationController : MonoBehaviour
{
    [SerializeField] private List<LangFlagPair> supportedLanguages;

    // Start is called before the first frame update
    void Start()
    {
        TrySetSystemLanguage();
    }

    private void TrySetSystemLanguage()
    {
        var curSystemLangFlagPair = supportedLanguages.First(x => x.SystemLanguage == Application.systemLanguage);
        
        if (curSystemLangFlagPair == null) return;
        
        var systemLang = curSystemLangFlagPair.ISIlanguage;

        if (ISILocalization.CurrentLanguage == systemLang) return;

        var localizedLanguage = ISILocalization.Instance.LocalizedLanguages.First(model => model.Language == systemLang);
        if (localizedLanguage != null)
        {
            ISILocalization.ChangeLanguage(systemLang);
        }
    }

    [Serializable]
    class LangFlagPair
    {
        public SystemLanguage SystemLanguage;
        public Language ISIlanguage;
    }
}
