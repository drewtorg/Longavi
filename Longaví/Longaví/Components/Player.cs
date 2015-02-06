using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Longaví.TileEngine;
using Longaví.Sprite;

namespace Longaví.Components
{
    public class Player
    {
        #region Fields

        Camera camera;
        Game1 gameRef;
        readonly AnimatedSprite sprite;

        #endregion

        #region Properties

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        public AnimatedSprite Sprite
        {
            get { return sprite; }
        }

        #endregion

        #region Constructors

        public Player(Game game, AnimatedSprite sprite)
        {
            gameRef = (Game1)game;
            camera = new Camera(gameRef.ScreenRectangle);
            this.sprite = sprite;
        }

        #endregion

        #region Methods

        public void Update(GameTime gameTime)
        {
            camera.Update(gameTime);
            sprite.Update(gameTime);

            if (InputHandler.KeyReleased(Keys.Q))
            {
                Camera.ZoomIn();
                if (Camera.CameraMode == CameraMode.Follow)
                    Camera.LockToSprite(sprite);
            }

            else if (InputHandler.KeyReleased(Keys.E))
            {
                Camera.ZoomOut();
                if (Camera.CameraMode == CameraMode.Follow)
                    Camera.LockToSprite(sprite);
            }

            Vector2 motion = new Vector2();

            if (InputHandler.KeyDown(Keys.W))
            {
                sprite.CurrentAnimation = AnimationKey.Up;
                motion.Y = -1;
            }

            else if (InputHandler.KeyDown(Keys.S))
            {
                sprite.CurrentAnimation = AnimationKey.Down;
                motion.Y = 1;
            }

            if (InputHandler.KeyDown(Keys.A))
            {
                sprite.CurrentAnimation = AnimationKey.Left;
                motion.X = -1;
            }

            else if (InputHandler.KeyDown(Keys.D))
            {
                sprite.CurrentAnimation = AnimationKey.Right;
                motion.X = 1;
            }

            if (motion != Vector2.Zero)
            {
                sprite.IsAnimating = true;
                motion.Normalize();

                sprite.Position += motion * sprite.Speed;
                sprite.LockToMap();

                if (Camera.CameraMode == CameraMode.Follow)
                    Camera.LockToSprite(sprite);
            }

            else
                sprite.IsAnimating = false;

            if (InputHandler.KeyReleased(Keys.F))
            {
                Camera.ToggleCameraMode();
                if (Camera.CameraMode == CameraMode.Follow)
                    Camera.LockToSprite(sprite);
            }

            if (Camera.CameraMode != CameraMode.Follow)
            {
                if (InputHandler.KeyReleased(Keys.C))
                    Camera.LockToSprite(sprite);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, camera);
        }

        #endregion
    }
}
