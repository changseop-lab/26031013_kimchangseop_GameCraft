
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
    private G2Texture? stage2Image;
    private G2Texture? stage3Image;
    private G2Texture? player;

    private G2AudioMp3? titleBgm;
    private G2AudioMp3? stage1;

    private G2Font? font;

    private int page = 0;

    private float x;
    private float y;
    private float moveSpeed = 250;

    private float depth = 0;
    private float cameraY = 0;

    private Rect button;

    protected override void Initialize()
    {
        titleImage = new("resource/image/background/타이틀 화면 배경.png");
        startImage = new("resource/image/font/게임 시작.png");
        waitingImage = new("resource/image/background/게임 시작 시 대기 화면.png");

        stage1Image = new("resource/image/background/1스테이지 배경.png");
        stage2Image = new("resource/image/background/2스테이지 배경.png");
        stage3Image = new("resource/image/background/3스테이지 배경.png");

        player = new("resource/image/object/플레이어.png");

        titleBgm = new("resource/sound/bgm/타이틀 화면.mp3");
        stage1 = new("resource/sound/bgm/1,2 스테이지.mp3");

        font = new G2Font("Malgun Gothic", 26);

        titleBgm.Play(true);

        button = new Rect(
            ScreenSize.Width / 2f - 150,
            ScreenSize.Height * 0.65f,
            300,
            120);

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

        // 대각선 이동도 같은 속도가 되도록 보정
        float length = MathF.Max(
            1,
            MathF.Sqrt(dx * dx + dy * dy));

        float distance = moveSpeed * (float)DeltaTime;

        x += dx / length * distance;
        y += dy / length * distance;

        // 대기 화면 아래쪽에 도달하면 스테이지 진입
        if (page == 1 && y + 100 >= ScreenSize.Height)
        {
            page = 2;

            y = 0;
            depth = 0;
            cameraY = 0;

            titleBgm?.Stop();
            stage1?.Play(true);
        }

        x = Math.Clamp(
            x,
            0,
            Math.Max(0, ScreenSize.Width - 88));

        if (page == 1)
        {
            y = Math.Clamp(
                y,
                0,
                Math.Max(0, ScreenSize.Height - 100));
        }
        else if (page == 2)
        {
            // 스테이지 진입 위치보다 위로 올라가지 못하도록 제한
            y = Math.Max(0, y);

            // 플레이어 실제 위치로 수심 계산
            // 배경 한 장의 출력 높이만큼 내려가면 100m
            depth = (float)Math.Round(
                (double)y / ScreenSize.Height * 100,
                3);

            // 플레이어가 화면 중앙에 도달하면 카메라가 따라감
            cameraY = Math.Max(
                0,
                y - (ScreenSize.Height - 100) / 2f);
        }
    }

    protected override void Render()
    {
        Rect screen = new Rect(
            0,
            0,
            ScreenSize.Width,
            ScreenSize.Height);

        if (page == 0)
        {
            titleImage?.Draw(
                screen,
                new Rect(0, 0, 1672, 941));

            startImage?.Draw(
                button,
                new Rect(0, 0, 1986, 792));

            return;
        }

        if (page == 1)
        {
            waitingImage?.Draw(
                screen,
                new Rect(0, 0, 1672, 941));
        }
        else
        {
            G2Texture? background = stage1Image;

            // 현재 수심에 맞는 배경 선택
            if (depth >= 2001f)
            {
                background = stage3Image;
            }
            else if (depth >= 1001f)
            {
                background = stage2Image;
            }

            // 카메라 이동에 맞춰 배경 반복
            float offset = cameraY % ScreenSize.Height;

            background?.Draw(
                new Rect(
                    0,
                    -offset,
                    ScreenSize.Width,
                    ScreenSize.Height),
                new Rect(0, 0, 1672, 941));

            background?.Draw(
                new Rect(
                    0,
                    ScreenSize.Height - offset,
                    ScreenSize.Width,
                    ScreenSize.Height),
                new Rect(0, 0, 1672, 941));
        }

        // 실제 위치에서 카메라 위치를 빼서 화면에 표시
        player?.Draw(
            new Rect(x, y - cameraY, 88, 100),
            new Rect(0, 0, 1176, 1337));

        if (page == 2)
        {
            font?.DrawText(
                "수심 " + ((int)depth).ToString() + "m",
                new Rect(
                    20,
                    20,
                    ScreenSize.Width - 40,
                    50),
                new Color4(1f, 1f, 1f, 1f));
        }
    }

    public override void Dispose()
    {
        titleBgm?.Stop();
        stage1?.Stop();

        titleImage?.Dispose();
        startImage?.Dispose();
        waitingImage?.Dispose();

        stage1Image?.Dispose();
        stage2Image?.Dispose();
        stage3Image?.Dispose();

        player?.Dispose();
        font?.Dispose();

        titleBgm?.Dispose();
        stage1?.Dispose();

        base.Dispose();
    }
}