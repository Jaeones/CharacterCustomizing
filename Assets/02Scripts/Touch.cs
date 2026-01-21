using UnityEngine;

public class Touch : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Touch", false); // 애니메이션 초기화
        anim.SetBool("TouchHead", false);
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //만약 터치한 오브젝트 태그가 "Head" 라면 
                if (hit.collider.tag == "Head")
                {
                    Debug.Log("Head");
                    anim.SetBool("TouchHead", true);
                }
                //만약 터치한 오브젝트 태그가 "Body" 라면 
                else if (hit.collider.tag == "Body")
                {
                    Debug.Log("Body");
                    anim.SetBool("Touch", true);
                }
                else
                {
                    anim.SetBool("Touch", true);
                }
            }
        }
    }
}
