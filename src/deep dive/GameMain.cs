
using System.Windows.Forms;
using Vortice.Mathematics;

using Vortice.Mathematics;

using Vortice.Mathematics;

using Vortice.Mathematics;

using System;
using System.Windows.Forms;
using Vortice.Mathematics;

using System;
using System.Windows.Forms;
using Vortice.Mathematics;

using System;
using System.Windows.Forms;
using Vortice.Mathematics;

class GameMain : G2AppBase
{
    public override System.Drawing.Size ScreenSize => GameGlobal.ScreenSize;
    public override string GameName => GameGlobal.GameName;

    private G2Texture? titleImage;
    private G2Texture? startImage;
    private G2Texture? waitingImage;
    private G2Texture? stage1Image;
    private G2Texture? player;

    private G2AudioMp3? titleBgm;
    private G2AudioMp3? stage1;

    private int page = 0; 
    private float x;
    private float y;
    private float moveSpeed = 250;
    private Rect button;

    protected override void Initialize()
    {
        titleImage = new("resource/image/background/타이틀 화면 배경.png");
        startImage = new("resource/image/font/게임 시작.png");
        waitingImage = new("resource/image/background/게임 시작 시 대기 화면.png");
        stage1Image = new("resource/image/background/1스테이지 배경.png");
        player = new("resource/image/object/플레이어.png");

        titleBgm = new("resource/sound/bgm/타이틀 화면.mp3");
        stage1 = new("resource/sound/bgm/1,2 스테이지.mp3");
        titleBgm.Play(true);

        button = new Rect(
            ScreenSize.Width / 2f - 150,
            ScreenSize.Height * 0.65f,
            300, 120);

        x = (ScreenSize.Width - 88) / 2f;
        y = (ScreenSize.Height - 100) / 2f;
    }

    private bool Held(Keys key)
    {
        return Input.IsKeyDown(key) || Input.IsKeyPress(key);
    }

    protected override void Update()
    {
        if (page == 0)
        {
            var mouse = Input.MousePosition;

            if (Input.IsButtonDown(MouseButtons.Left) &&
                mouse.X >= button.X &&
                mouse.X <= button.X + button.Width &&
                mouse.Y >= button.Y &&
                mouse.Y <= button.Y + button.Height)
            {
                page = 1;
            }

            return;
        }

        float dx = 0;
        float dy = 0;

        if (Held(Keys.A)) dx--;
        if (Held(Keys.D)) dx++;
        if (Held(Keys.W)) dy--;
        if (Held(Keys.S)) dy++;

        
        float length = MathF.Max(1, MathF.Sqrt(dx * dx + dy * dy));
        float distance = moveSpeed * (float)DeltaTime;

        x += dx / length * distance;
        y += dy / length * distance;

        
        if (page == 1 && y + 100 >= ScreenSize.Height)
        {
            page = 2;
            y = 0;

            titleBgm?.Stop();
            stage1?.Play(true);
        }

        x = Math.Clamp(x, 0, Math.Max(0, ScreenSize.Width - 88));
        y = Math.Clamp(y, 0, Math.Max(0, ScreenSize.Height - 100));
    }

    protected override void Render()
    {
        Rect screen = new Rect(
            0, 0, ScreenSize.Width, ScreenSize.Height);

        if (page == 0)
        {
            titleImage?.Draw(screen, new Rect(0, 0, 1672, 941));
            startImage?.Draw(button, new Rect(0, 0, 1986, 792));
            return;
        }

        if (page == 1)
            waitingImage?.Draw(screen, new Rect(0, 0, 1672, 941));
        else
            stage1Image?.Draw(screen, new Rect(0, 0, 1672, 941));

        player?.Draw(
            new Rect(x, y, 88, 100),
            new Rect(0, 0, 1176, 1337));
    }

    public override void Dispose()
    {
        titleBgm?.Stop();
        stage1?.Stop();

        titleImage?.Dispose();
        startImage?.Dispose();
        waitingImage?.Dispose();
        stage1Image?.Dispose();
        player?.Dispose();

        titleBgm?.Dispose();
        stage1?.Dispose();

        base.Dispose();
    }
}