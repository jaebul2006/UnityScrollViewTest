using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ScrollViewAdapterTest : MonoBehaviour
{
    public RectTransform _prefab;
    public Text _count_text;
    public ScrollRect _scroll_view;
    public RectTransform _content;

    List<TestItemView> _views = new List<TestItemView>();

    public class TestItemView
    {
        public Text _title_text;

        public TestItemView(Transform root_view)
        {
            _title_text = root_view.Find("Text").GetComponent<Text>();
        }
    }

    public class TestItemModel
    {
        public string _title;
    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    // 버튼 클릭으로 호출됨
    public void UpdateItems()
    {
        int new_count = 0;
        int.TryParse(_count_text.text, out new_count);
        StartCoroutine(FetchModelsFromServer(new_count, results => OnReceiveNewModels(results)));
    }

    void OnReceiveNewModels(TestItemModel[] models)
    {
        foreach (Transform child in _content)
            Destroy(child.gameObject);

        _views.Clear();

        foreach(var model in models)
        {
            var instance = GameObject.Instantiate(_prefab.gameObject) as GameObject;
            instance.transform.SetParent(_content, false);
            var view = InitializeItemView(instance, model);
            _views.Add(view);
        }
    }

    TestItemView InitializeItemView(GameObject view_gameobject, TestItemModel model)
    {
        TestItemView view = new TestItemView(view_gameobject.transform);
        view._title_text.text = model._title;
        view_gameobject.name = model._title;
        return view;
    }

    IEnumerator FetchModelsFromServer(int count, Action<TestItemModel[]> onDone)
    {
        yield return new WaitForSeconds(1f);

        var results = new TestItemModel[count];
        for(int i=0; i<count; i++)
        {
            results[i] = new TestItemModel();
            results[i]._title = "itme" + i;
        }
        onDone(results);
    }

    // 개별 item 터치 처리
    public void TouchItem(GameObject obj)
    {
        Debug.Log(obj.name);
    }
       
}

// 오늘의 구현 목표
// 1. 다중 컬럼 스크롤뷰
// 2. 스크롤 아이템 선택, 해제