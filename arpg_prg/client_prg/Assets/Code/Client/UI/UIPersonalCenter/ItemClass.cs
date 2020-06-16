using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemClass : MonoBehaviour
{
    public class PersonalItem
    {
        public Transform o;
        public Image title;
        public Image win;
        public Text money;
        public Text timeCount;
        public Text quality;
        public Text time;
        public Button detail;

        public PersonalItem(Transform obj)
        {
            o = obj;
            title = obj.Find("Title").GetComponent<Image>();
            win = obj.Find("Win").GetComponent<Image>();
            money = obj.Find("Money").GetComponent<Text>();
            timeCount = obj.Find("TimeCount").GetComponent<Text>(); 
            quality = obj.Find("Quality").GetComponent<Text>();
            time = obj.Find("Time").GetComponent<Text>();
            detail = obj.Find("Detail").GetComponent<Button>();
        }
        public PersonalItem Clone()
        {
            GameObject obj = GameObject.Instantiate(o.gameObject);
            obj.transform.SetParent(o.parent, false);
            obj.transform.localPosition = Vector3.zero;
            return new PersonalItem(obj.transform);
        }

    }

    public class PersonalGameRecodeItem
    {
        public Transform o;
        public Image title;
        public Text name;
        public Text pro;
        public Text creat;
        public Text use;
        public Text mgr;
        public Text beyond;
        public Text all;

        public PersonalGameRecodeItem(Transform obj)
        {
            o = obj;
            title = obj.Find("Title").GetComponent<Image>();
            name = obj.Find("Name").GetComponent<Text>();
            pro = obj.Find("Pro").GetComponent<Text>();
            creat = obj.Find("Creat").GetComponent<Text>();
            use = obj.Find("Use").GetComponent<Text>();
            mgr = obj.Find("Mgr").GetComponent<Text>();
            beyond = obj.Find("Beyond").GetComponent<Text>();
            all = obj.Find("All").GetComponent<Text>();
        }
        public PersonalGameRecodeItem Clone()
        {
            GameObject obj = GameObject.Instantiate(o.gameObject);
            obj.transform.SetParent(o.parent, false);
            obj.transform.localPosition = Vector3.zero;
            return new PersonalGameRecodeItem(obj.transform);
        }

    }

        public class PersonalGameRecodeList
        {
            public Transform o;
            public Image title;
            public Text name;
            public Image win;
            public Text money;
            public Text timeCount;
            public Text quality;
            public Text time;
            public Button detail;

            public PersonalGameRecodeList(Transform obj)
            {
                o = obj;
                o = obj;
                title = obj.Find("Title").GetComponent<Image>();
                name = obj.Find("Name").GetComponent<Text>();
                win = obj.Find("Win").GetComponent<Image>();
                money = obj.Find("Money").GetComponent<Text>();
                timeCount = obj.Find("TimeCount").GetComponent<Text>();
                quality = obj.Find("Quality").GetComponent<Text>();
                time = obj.Find("Time").GetComponent<Text>();
                detail = obj.Find("Detail").GetComponent<Button>();
            }
            public PersonalGameRecodeList Clone()
            {
                GameObject obj = GameObject.Instantiate(o.gameObject);
                obj.transform.SetParent(o.parent, false);
                obj.transform.localPosition = Vector3.zero;
                return new PersonalGameRecodeList(obj.transform);
            }
        }  
}
