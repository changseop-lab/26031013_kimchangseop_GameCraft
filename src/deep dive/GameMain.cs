
using Vortice.Mathematics;

class GameMain : G2AppBase
{
    public override System.Drawing.Size ScreenSize => GameGlobal.ScreenSize;
    public override string GameName => GameGlobal.GameName;




    protected override void Initialize()
    {
        //---------------------------------------
        // 게임 관련 객체를 생성합니다.
        //---------------------------------------
    }

    protected override void Update()
    {
        //---------------------------------------
        // 게임 관련 객체를 갱신합니다.
        //---------------------------------------
    }

    protected override void Render()
    {
        //---------------------------------------
        // 게임 관련 객체를 렌더링합니다.
        //---------------------------------------
    }

    public override void Dispose()
    {
        base.Dispose();

        //---------------------------------------
        // 게임 관련 객체를 해제합니다.
        //---------------------------------------
    }
}
