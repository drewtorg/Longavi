using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Longaví.TileEngine;
using Longaví.Components;
using Longaví.Sprite;

namespace Longaví.GameScreens
{
    public class GamePlayScreen : BaseGameState
    {
        #region Fields

        Engine engine = new Engine(32, 32);
        TileMap map;
        Player player;
        const int MAP_HEIGHT = 100;
        const int MAP_WIDTH = 100;

        #endregion

        #region Properties
        #endregion

        #region Constructor

        public GamePlayScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Texture2D spriteSheet = Game.Content.Load<Texture2D>(@"Sprites\jack");
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey,Animation>();

            Animation animation = new Animation(3, 16, 24, 0, 0);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(3, 16, 24, 0, 24);
            animations.Add(AnimationKey.Left, animation);

            animation = new Animation(3, 16, 24, 0, 48);
            animations.Add(AnimationKey.Up, animation);

            animation = new Animation(3, 16, 24, 0, 72);
            animations.Add(AnimationKey.Right, animation);

            AnimatedSprite sprite = new AnimatedSprite(spriteSheet, animations);

            player = new Player(GameRef, sprite);

            base.LoadContent();

            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Tileset\tileset3");
            Tileset tileset1 = new Tileset(tilesetTexture, 16, 16, 32, 32);

            MapLayer layer = new MapLayer(MAP_WIDTH, MAP_HEIGHT);

            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    Tile tile = new Tile(78, 0);

                    layer.SetTile(x, y, tile);
                }
            }

            map = new TileMap(tileset1, layer);

            MapLayer splatter = new MapLayer(MAP_WIDTH, MAP_HEIGHT);

            Random randy = new Random();

            for (int i = 0; i < MAP_WIDTH * 3; i++)
            {
                int x = randy.Next(0, MAP_WIDTH);
                int y = randy.Next(0, MAP_HEIGHT);
                int index = randy.Next(0, 4);
                Tile tile;

                switch(index)
                {
                    case 0: index = 151; break;
                    case 1: index = 167; break;
                    case 2: index = 168; break;
                    case 3: index = 204; break;
                }

                tile = new Tile(index, 0);
                splatter.SetTile(x, y, tile);
            }

            map.AddLayer(splatter);
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                player.Camera.Transformation);

            map.Draw(GameRef.SpriteBatch, player.Camera);

            player.Draw(gameTime, GameRef.SpriteBatch);

            base.Draw(gameTime);

            GameRef.SpriteBatch.End();
        }

        #endregion

        #region Abstract Method Region
        #endregion
    }
}
