using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Glamour2
{

    class Sprite
    {

        public string name;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 size;

        public AnimationHandler animHandler;

        public Vector2 feetPosition
        {
            get
            {
                return new Vector2(position.X, position.Y + size.Y);
            }
        }

        public bool facingRight;

        public bool visible;

        public Sprite(string filename, ContentManager Content, Vector2 vec, bool animated = true)
        {
            name = filename;
            texture = Content.Load<Texture2D>(filename);


            visible = true;

            this.position = vec;
            if (animated)
            {
                SpriteMetaInfo smi = new SpriteMetaInfo(filename);
                size = new Vector2(smi.width, smi.height);
                this.animHandler = new AnimationHandler(smi.height, smi.width, smi.frames, smi.durations, smi.loops);
            }
            else
            {
                size = new Vector2(texture.Width, texture.Height);
                this.animHandler = new AnimationHandler(texture.Height, texture.Width, new List<int>(), new List<int>(), new List<bool>());
            }
            this.animHandler.setAnimation(0, true);

        }

        public virtual void update(double dt, bool perspective = true)
        {

            this.animHandler.update(dt);
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            if (!visible) return;
            if (facingRight)
            {
                spriteBatch.Draw(this.texture, position: this.position,
                    sourceRectangle: this.animHandler.textureSection,
                    effects: SpriteEffects.FlipHorizontally, color: Color.White);
            }
            else
            {
                spriteBatch.Draw(this.texture, position: this.position,
                   sourceRectangle: this.animHandler.textureSection, color: Color.White);
            }
        }

        // overwrite position
        public void draw(SpriteBatch spriteBatch, Vector2 pos, Color color)
        {
            if (!visible) return;
            if (facingRight)
            {
                spriteBatch.Draw(this.texture, position: pos,
                    sourceRectangle: this.animHandler.textureSection,
                    effects: SpriteEffects.FlipHorizontally, color: color);
            }
            else
            {
                spriteBatch.Draw(this.texture, position: pos,
                   sourceRectangle: this.animHandler.textureSection, color: color);
            }
        }

    }

    public class SpriteMetaInfo
    {

        public int height;
        public int width;
        public List<int> frames;
        public List<int> durations;
        public List<bool> loops;
        public bool collides;


        public SpriteMetaInfo(String texture)
        {
            long start = DateTime.Now.Ticks;
            frames = new List<int>();
            durations = new List<int>();
            loops = new List<bool>();
            StreamReader sr = new StreamReader("Content/" + texture + "_meta");
            height = int.Parse(sr.ReadLine());
            width = int.Parse(sr.ReadLine());
            int nAnimations = int.Parse(sr.ReadLine());
            string frLine = sr.ReadLine();
            string duLine = sr.ReadLine();
            string loLine = sr.ReadLine();
            string[] frStrings = frLine.Split(',');
            string[] duStrings = duLine.Split(',');
            string[] loStrings = loLine.Split(',');
            for (int x = 0; x < nAnimations; x++)
            {
                frames.Add(int.Parse(frStrings[x]));
                durations.Add(int.Parse(duStrings[x]));
                loops.Add((int.Parse(loStrings[x]) == 1));
            }
            sr.Close();
            long stop = DateTime.Now.Ticks;
            // Console.WriteLine("Loaded animations for " + texture + " in " + (stop - start) / TimeSpan.TicksPerMillisecond + "ms.");
        }

    }
}

