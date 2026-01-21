using UnityEngine;
using UnityEngine.UI;

public class RPSGame : MonoBehaviour
{
    bool isRPS;
    int modeRPS = 0;

    //상수 const -> enum
    //선언할 때만 초기화 할 수 있다
    //가위바위보 상태를 나타내는 상수
    const int RPS = 0;
    const int ROCK = 1;
    const int PAPER = 2;
    const int SCISSORS = 3;

    //가위바위보 결과를 나타내는 상수
    const int DRAW = 4;
    const int WIN = 5;
    const int LOSE = 6;

    int myHand;
    int enemyHand;

    int result;
    float waitTime;

    public GameObject gameStartPanel;
    public GameObject rpsPanel;
    public Text win;
    public Text lose;
    public Text draw;

    Animator enemyAnimator;   // 적 애니메이터

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        gameStartPanel.SetActive(true);
        rpsPanel.SetActive(false);
        win.enabled = false;
        lose.enabled = false;
        draw.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //isRPS true 라면
        //switch문으로 가위바위보 모드 시작
        //switch문으로 비교할 변수 : modeRPS
        // modeRPS = 0 : 가위바위보 시작
        // modeRPS = 1 : 플레이어의 입력대기
        // modeRPS = 2 : 판정
        // modeRPS = 3 : 결과
        // modeRPS = 4 : 게임 다시 시작. (씬전환X, 리셋)
        // 플레이어의 가위바위보는 버튼 이벤트로 입력받는다
        if (isRPS)
        {
            switch (modeRPS)
            {
                case 0:
                    // 가위바위보 시작
                    GameStart();
                    break;
                case 1:
                    // 플레이어의 입력 대기
                    // 버튼 입력을 기다림 (OnPlayerChoice 메서드가 호출될 때까지 대기)
                    break;
                case 2:
                    // 판정
                    JudgeGame();
                    break;
                case 3:
                    // 결과
                    ShowResult();
                    break;
                case 4:
                    // 게임 다시 시작
                    ResetGame();
                    break;
            }
        }
    }

    // 게임 시작 메서드
    void GameStart()
    {
        Debug.Log("가위바위보 시작!");
        myHand = 0;
        enemyHand = 0;
        result = 0;
        modeRPS = 1; // 플레이어 입력 대기 모드로 전환
        
        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("RPS", true);
        }
    }

    // 버튼에서 호출할 메서드들 (UI 버튼에 연결)
    public void OnPlayerChoiceRock()
    {
        OnPlayerChoice(ROCK);
    }

    public void OnPlayerChoicePaper()
    {
        OnPlayerChoice(PAPER);
    }

    public void OnPlayerChoiceScissors()
    {
        OnPlayerChoice(SCISSORS);
    }

    // 플레이어 선택 처리
    void OnPlayerChoice(int choice)
    {
        if (modeRPS != 1) return; // 입력 대기 상태가 아니면 무시

        myHand = choice;
        

        // 적의 랜덤 선택
        enemyHand = Random.Range(ROCK, SCISSORS + 1);
       

        // 적 애니메이션 재생
        PlayEnemyAnimation(enemyHand);

        // 판정 모드로 전환
        modeRPS = 2;
        waitTime = 0f;
    }

    // 판정 로직
    void JudgeGame()
    {
        // 애니메이션이 재생되는 동안 잠시 대기
        waitTime += Time.deltaTime;
        if (waitTime < 1.5f) return; // 1.5초 대기

        if (myHand == enemyHand)
        {
            result = DRAW;
            if (enemyAnimator != null)
                enemyAnimator.SetBool("Draw", true);
            Debug.Log("무승부!");
            draw.enabled = true;
        }
        else if ((myHand == ROCK && enemyHand == SCISSORS) ||
                 (myHand == PAPER && enemyHand == ROCK) ||
                 (myHand == SCISSORS && enemyHand == PAPER))
        {
            result = WIN;
            if (enemyAnimator != null)
                enemyAnimator.SetBool("Lose", true);
            Debug.Log("승리!");
            win.enabled = true;
        }
        else
        {
            result = LOSE;
            if (enemyAnimator != null)
                enemyAnimator.SetBool("Win", true);
            Debug.Log("패배!");
            lose.enabled = true;
        }

        modeRPS = 3; // 결과 표시 모드로 전환
        waitTime = 0f;
    }

    // 결과 표시
    void ShowResult()
    {
        // 결과를 표시하고 일정 시간 대기
        waitTime += Time.deltaTime;
        if (waitTime < 2.0f) return; // 2초 대기

        waitTime = 0f; // ResetGame에서 타이밍을 다시 측정하기 위해 리셋
        modeRPS = 4; // 게임 다시 시작 모드로 전환
    }

    // 게임 리셋
    void ResetGame()
    {
        waitTime += Time.deltaTime;
        
        // 첫 번째 단계: Win/Lose/Draw 애니메이션이 완전히 종료되도록 먼저 false로 설정
        if (waitTime < 1.5f)
        {
            if (enemyAnimator != null)
            {
                enemyAnimator.SetBool("Draw", false);
                enemyAnimator.SetBool("Win", false);
                enemyAnimator.SetBool("Lose", false);
                enemyAnimator.SetBool("Rock", false);
                enemyAnimator.SetBool("Paper", false);
                enemyAnimator.SetBool("Scissors", false);
            }
            return;
        }
        
        // 두 번째 단계: RPS를 false로 설정하고 애니메이션이 완전히 종료될 때까지 대기
        if (waitTime < 3.0f)
        {
            if (enemyAnimator != null)
            {
                enemyAnimator.SetBool("RPS", false);
            }
            return;
        }
        
        // 세 번째 단계: 모든 리셋 완료, 다음 게임 시작 준비
        Debug.Log("게임 다시 시작");
        myHand = 0;
        enemyHand = 0;
        result = 0;
        waitTime = 0f;
        modeRPS = 0; // 초기 상태로 돌아감
        win.enabled = false;
        lose.enabled = false;
        draw.enabled = false;
    }

    // 적 애니메이션 재생
    void PlayEnemyAnimation(int hand)
    {
        if (enemyAnimator == null) return;

        // 이전 손 선택 애니메이션 모두 false로 설정
        enemyAnimator.SetBool("Rock", false);
        enemyAnimator.SetBool("Paper", false);
        enemyAnimator.SetBool("Scissors", false);

        switch (hand)
        {
            case ROCK:
                enemyAnimator.SetBool("Rock", true);
                break;
            case PAPER:
                enemyAnimator.SetBool("Paper", true);
                break;
            case SCISSORS:
                enemyAnimator.SetBool("Scissors", true);
                break;
        }
    }
    

    // 게임 시작 버튼에서 호출할 메서드
    public void StartRPSGame()
    {
        isRPS = true;
        modeRPS = 0;
        gameStartPanel.SetActive(false);
        rpsPanel.SetActive(true);
    }

    // 게임 종료 버튼에서 호출할 메서드
    public void StopRPSGame()
    {
        isRPS = false;
        ResetGame();
        gameStartPanel.SetActive(true);
        rpsPanel.SetActive(false);
        enemyAnimator.SetBool("RPS", false);
    }
}
