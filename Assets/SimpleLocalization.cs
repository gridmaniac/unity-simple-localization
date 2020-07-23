using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SimpleLocalization : MonoBehaviour {

    public SystemLanguage fallbackLanguage = SystemLanguage.English;

    private Dictionary<SystemLanguage, Dictionary<string, string>> s; SystemLanguage e; SystemLanguage cc;
    private delegate void DA(string key, string value); DA push;
    private delegate void PL(SystemLanguage lang, string term, int i); PL set;
    private delegate void IL(SystemLanguage lang); IL init;

    //Set locals here
    void SetupLanguages()
    {
        #region English
        init(SystemLanguage.English);

        push("TEST_STRING_1", "English word number 1");
        push("TEST_STRING_2", "English word number 2");
        #endregion

        #region Russian
        init(SystemLanguage.Russian);

        push("TEST_STRING_1", "Привет из первого");
        push("TEST_STRING_2", "Досвидания из второго");
        #endregion
    }

    void Awake()
    {
        s = new Dictionary<SystemLanguage, Dictionary<string, string>>();
        push = delegate (string key, string value) { s[e].Add(key, value); };

        //cc = Application.systemLanguage;
        cc = SystemLanguage.English;
        
        init = delegate (SystemLanguage lang) { e = lang; s[e] = new Dictionary<string, string>(); };

        SetupLanguages();

        var terms = FindObjectsOfType<Text>();
        if (!s.ContainsKey(cc)) cc = SystemLanguage.English;

        set = delegate (SystemLanguage lang, string term, int i)
        {
            string o;
            if (s[lang].TryGetValue(term, out o)) { terms[i].text = o; }
            else { terms[i].text = term; }
        };

        for (var i = 0; i < terms.Length; i++)
        {
            var test = Regex.Match(terms[i].text, "{(.*?)}");
            if (test.Success)
            {
                var term = test.Groups[1].Value;
                if (s[cc].ContainsKey(term)) { set(cc, term, i); }  
                else { set(fallbackLanguage, term, i); }
            }
            
        }
    }
}
