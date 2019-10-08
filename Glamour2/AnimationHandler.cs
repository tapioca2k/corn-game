using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace Glamour2
{
	class AnimationHandler
	{
		private List<int> frames;
		private List<int> durations;
        private List<bool> loops;
		private int currentFrame;
		private double t;
		private int spriteWidth;
		private int spriteHeight;

		public int currentAnimation;
        public bool animationFinished;
		public Microsoft.Xna.Framework.Rectangle textureSection;


        private bool ignoreAnimationChange;

		public AnimationHandler (int height, int width, List<int> frameCounts, List<int> frameDurations, List<bool> willLoop)
		{
            ignoreAnimationChange = false;
			spriteHeight = height;
			spriteWidth = width;
			frames = frameCounts;
			durations = frameDurations;
			textureSection = new Microsoft.Xna.Framework.Rectangle (0, 0, width, height);
			t = 0;
            loops = willLoop;
		}

		public int animationCount()
		{
			return durations.Count;
		}

		public void setAnimation(int n, bool resetTime)
		{
			if (n < 0 || n > this.animationCount () - 1 || ignoreAnimationChange)
				return;
			currentAnimation = n;
            animationFinished = false;
			textureSection = new Microsoft.Xna.Framework.Rectangle (currentFrame * (spriteWidth+1), spriteHeight * n, spriteWidth, spriteHeight);
			if (resetTime) {
				t = 0;
				currentFrame = 0;
                textureSection = new Microsoft.Xna.Framework.Rectangle(0, spriteHeight * n, spriteWidth, spriteHeight);
			} else if (currentFrame >= frames [currentAnimation]) {
				currentFrame = 0;
                textureSection = new Microsoft.Xna.Framework.Rectangle(0, spriteHeight * n, spriteWidth, spriteHeight);
			}
		}

		public void update(double dt)
		{
			if (animationCount() == 0 || (currentFrame >= frames[currentAnimation] - 1 && !loops[currentAnimation])) {
                animationFinished = true;
				return;
			}
			int nextFrameT = (currentFrame + 1) * durations [currentAnimation];
			if (t + dt >= nextFrameT)
			{	
				currentFrame++;
                textureSection = new Microsoft.Xna.Framework.Rectangle(textureSection.X + spriteWidth + currentFrame, textureSection.Y, spriteWidth, spriteHeight);
			}

			if (currentFrame > frames [currentAnimation] - 1 && loops[currentAnimation])
			{
				currentFrame = 0;
                textureSection = new Microsoft.Xna.Framework.Rectangle(0, textureSection.Y, spriteWidth, spriteHeight);
				t = 0;
			}

			t += dt;

		}


	}
}

