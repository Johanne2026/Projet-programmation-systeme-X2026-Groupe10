using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace restaurantProject1._0
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D chefTexture;
        private List<Vector2> chefWaypoints;
        private Vector2 chefPosition;
        private int currentWaypointIndex;

        private Texture2D maitreHotelTexture;
        private Vector2 maitreHotelPosition;
        private Vector2 maitreHotelInitialPosition;

        private Texture2D serveurTexture;
        private Vector2 serveurPosition;
        private Vector2 serveurInitialPosition;

        private Texture2D client1Texture;
        private Vector2 client1Position;
        private Vector2 client1InitialPosition;
        private bool client1Active;

        private Texture2D client2Texture;
        private Vector2 client2Position;
        private Vector2 client2InitialPosition;
        private bool client2Active;

        private bool servingStarted;
        private Vector2 servingPoint;
        private Vector2 servingTarget;

        private Texture2D chefRangTexture;
        private Vector2 chefRangPosition;
        private Vector2 chefRangInitialPosition;

        // Variable pour la carte
        private Texture2D mapTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the chef texture
            chefTexture = Content.Load<Texture2D>("chefcuisinier");

            // Define the waypoints for the chef's movement
            chefWaypoints = new List<Vector2>
            {
                new Vector2(100, 200),
                new Vector2(300, 150),
                new Vector2(500, 250),
                new Vector2(400, 400),
                new Vector2(200, 350)
            };
            chefPosition = chefWaypoints[0];
            currentWaypointIndex = 0;
            // Load chef de rang
            chefRangTexture = Content.Load<Texture2D>("chefRang");
            chefRangPosition = new Vector2(800, 600);
            chefRangInitialPosition = chefRangPosition;

            // Load the maître d'hôtel texture
            maitreHotelTexture = Content.Load<Texture2D>("maitrehotel");
            maitreHotelPosition = new Vector2(200, 200);
            maitreHotelInitialPosition = maitreHotelPosition;

            // Load the serveur texture
            serveurTexture = Content.Load<Texture2D>("serveur");
            serveurPosition = new Vector2(400, 200);
            serveurInitialPosition = serveurPosition;

            // Load element1 texture
            client1Texture = Content.Load<Texture2D>("client1");
            client1InitialPosition = new Vector2(600, 200);
            client1Position = client1InitialPosition;
            client1Active = false;

            // Load element2 texture
            client2Texture = Content.Load<Texture2D>("client2");
            client2InitialPosition = new Vector2(600, 400);
            client2Position = client2InitialPosition;
            client2Active = false;

            servingStarted = false;
            servingPoint = new Vector2(400, 300); // Point prédéfini où les éléments et le maitre d'hôtel se croisent
            servingTarget = new Vector2(800, 300); // Nouvelle position prédéfinie où les éléments et le serveur se déplacent

            // Load the map texture
            mapTexture = Content.Load<Texture2D>("map");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!servingStarted)
            {
                // Update the chef's position
                UpdateChef();

                // Move the maître d'hôtel horizontally
                maitreHotelPosition.X += 1f;

                // Check if the maître d'hôtel and the elements should become active
                if (maitreHotelPosition.X >= servingPoint.X)
                {
                    client1Active = true;
                    client2Active = true;
                }

                // Move the elements towards the serving point
                if (client1Active)
                {
                    Vector2 direction = Vector2.Normalize(servingPoint - client1Position);
                    float speed = 1f; // Adjust the speed as needed
                    client1Position += direction * speed;
                }

                if (client2Active)
                {
                    Vector2 direction = Vector2.Normalize(servingPoint - client2Position);
                    float speed = 1f; // Adjust the speed as needed
                    client2Position += direction * speed;
                }

                // Check if the maître d'hôtel and the elements have reached the serving point
                if (maitreHotelPosition.X >= servingPoint.X &&
                    Vector2.Distance(maitreHotelPosition, servingPoint) < 1f &&
                    Vector2.Distance(client1Position, servingPoint) < 1f &&
                    Vector2.Distance(client2Position, servingPoint) < 1f)
                {
                    servingStarted = true;
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                if (!servingStarted)
                {
                    // Update the chef's position
                    UpdateChef();

                    // Move the maître d'hôtel horizontally
                    maitreHotelPosition.X += 1f;

                    // Check if the maître d'hôtel and the elements should become active
                    if (maitreHotelPosition.X >= servingPoint.X)
                    {
                        client1Active = true;
                        client2Active = true;
                    }

                    // Move the elements towards the serving point
                    if (client1Active)
                    {
                        Vector2 direction = Vector2.Normalize(servingPoint - client1Position);
                        float speed = 1f; // Adjust the speed as needed
                        client1Position += direction * speed;
                    }

                    if (client2Active)
                    {
                        Vector2 direction = Vector2.Normalize(servingPoint - client2Position);
                        float speed = 1f; // Adjust the speed as needed
                        client2Position += direction * speed;
                    }

                    // Check if the maître d'hôtel and the elements have reached the serving point
                    if (maitreHotelPosition.X >= servingPoint.X &&
                        Vector2.Distance(maitreHotelPosition, servingPoint) < 1f &&
                        Vector2.Distance(client1Position, servingPoint) < 1f &&
                        Vector2.Distance(client2Position, servingPoint) < 1f)
                    {
                        servingStarted = true;
                    }
                }
                else
                {
                    // Move the serveur towards the serving target
                    Vector2 direction = Vector2.Normalize(servingTarget - serveurPosition);
                    float speed = 1f; // Adjust the speed as needed
                    serveurPosition += direction * speed;

                    // Check if the serveur has reached the serving target
                    if (Vector2.Distance(serveurPosition, servingTarget) < 1f)
                    {
                        // Reset positions and flags
                        maitreHotelPosition = maitreHotelInitialPosition;
                        serveurPosition = serveurInitialPosition;
                        client1Position = client1InitialPosition;
                        client2Position = client2InitialPosition;
                        client1Active = false;
                        client2Active = false;
                        servingStarted = false;
                    }
                }

                base.Update(gameTime);
            }

            void UpdateChef()
            {
                // Move the chef towards the current waypoint
                Vector2 direction = Vector2.Normalize(chefWaypoints[currentWaypointIndex] - chefPosition);
                float speed = 1f; // Adjust the speed as needed
                chefPosition += direction * speed;

                // Check if the chef has reached the current waypoint
                if (Vector2.Distance(chefPosition, chefWaypoints[currentWaypointIndex]) < 1f)
                {
                    // Move to the next waypoint
                    currentWaypointIndex = (currentWaypointIndex + 1) % chefWaypoints.Count;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // Draw the map
            Rectangle mapRectangle = new Rectangle(0, 0, 724, 483);
            spriteBatch.Draw(mapTexture, Vector2.Zero, mapRectangle, Color.White);
            // Draw the chefRang
            Rectangle chefRangRectangle = new Rectangle(608, 256, 30, 30);
            spriteBatch.Draw(chefRangTexture, Vector2.Zero, chefRangRectangle, Color.White);

            // Draw the chef
            Rectangle chefcuisinierRectangle = new Rectangle(256, 64, 30, 30);
            spriteBatch.Draw(chefTexture, chefcuisinierRectangle, Color.White);
            // Draw the maître d'hôtel
            Rectangle maitreHotelRectangle = new Rectangle(64, 224, 30, 30);
            spriteBatch.Draw(maitreHotelTexture, maitreHotelRectangle, Color.White);

            // Draw the serveur
            Rectangle serveurRectangle = new Rectangle(480, 128, 30, 30);
            spriteBatch.Draw(serveurTexture, serveurRectangle, Color.White);

            // Draw client1 if active
            Rectangle client1Rectangle = new Rectangle(0, 256, 30, 30);
            if (client1Active)
                spriteBatch.Draw(client1Texture, client1Rectangle, Color.White);

            // Draw client2 if active
            Rectangle client2Rectangle = new Rectangle(0, 288, 30, 30);
            if (client2Active)
                spriteBatch.Draw(client2Texture, client2Rectangle, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}