using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Client.UI;

public class PersonalItem
{
    public GameObject o;
    public RawImage title;
    public Text date;
    public Text money;
    public Text timeCount;
    public Text quality;
    public Text time;
    public Button detail;
    private ListData data;

    private UIRawImageDisplay _headImg;

    public PersonalItem(GameObject obj)
    {
        o = obj;

        var tmpTransform = o.GetComponent<RectTransform>();
        tmpTransform.sizeDelta = new Vector2(0, tmpTransform.sizeDelta.y);       

        title = obj.GetComponentEx<RawImage>("Title");
        date = obj.GetComponentEx<Text>("Date");
        money = obj.GetComponentEx<Text>("Money");
        timeCount = obj.GetComponentEx<Text>("TimeCount");
        quality = obj.GetComponentEx<Text>("Quality");
        time = obj.GetComponentEx<Text>("Time");
        detail = obj.GetComponentEx<Button>("Detail");
        _headImg = new UIRawImageDisplay(title);

        EventTriggerListener.Get(detail.gameObject).onClick = _ShowDetail;



    }
    public void Refresh(ListData value)
    {
        this.data = value;

       date.text = value.date;
       money.text = value.money;
       timeCount.text = value.timeCount;
       quality.text = value.quality;
       time.text = value.time;

        if(null!=_headImg)
        {
            _headImg.Load(value.headPath);
        }
    
    }

    private void _ShowDetail(GameObject go)
    {
        NetWorkScript.getInstance().GetRecordDetail(this.data.roomCode);

        var hallset = Client.UIControllerManager.Instance.GetController<UIPersonGameRecodeController>();
        hallset.setVisible(true);
    }

    public void Dispose()
    {
        if(null!=_headImg)
        {
            _headImg.Dispose();
        }
    }


    public PersonalItem Clone()
    {
        GameObject obj = GameObject.Instantiate(o);
        obj.transform.SetParent(o.transform.parent, false);
        obj.transform.localPosition = Vector3.zero;
        return new PersonalItem(obj);
    }

}

public class ItemClass : MonoBehaviour
{
    public class PersonalGameRecodeItem
    {
        public Transform o;
        public RawImage title;
        public Text name;
        public Text pro;
        public Text creat;
        public Text use;
        public Text mgr;
        public Text beyond;
        public Text all;

        private UIRawImageDisplay _headImg;

        public PersonalGameRecodeItem(Transform obj)
        {
            o = obj;
            title = obj.Find("Title").GetComponent<RawImage>();
            name = obj.Find("Name").GetComponent<Text>();
            pro = obj.Find("Pro").GetComponent<Text>();
            creat = obj.Find("Creat").GetComponent<Text>();
            use = obj.Find("Use").GetComponent<Text>();
            mgr = obj.Find("Mgr").GetComponent<Text>();
            beyond = obj.Find("Beyond").GetComponent<Text>();
            all = obj.Find("All").GetComponent<Text>();
            _headImg = new UIRawImageDisplay(title);
        }

        public void loadHead(string value)
        {
            if(null!=_headImg)
            {
                //Console.Error.WriteLine("---------===="+value);
                _headImg.Load(value);
            }

        }

        public void Dispose()
        {
            if(null!=_headImg)
            {
                _headImg.Dispose();
            }
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
