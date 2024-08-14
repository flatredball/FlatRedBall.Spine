using FlatRedBall.Graphics;
using FlatRedBall.Instructions;
using FlatRedBall.IO;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework.Graphics;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRedBall.Spine
{
    public class SpineDrawableBatch : PositionedObject, IDrawableBatch, IVisible
    {
        #region Fields/Properties

        Skeleton skeleton;
        AnimationState state;
        public AnimationState AnimationState
        {
            get => state;
            private set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Can't set AnimationState to null");
                }

                state = value;

                state.Event += (sender, args) => Event?.Invoke(sender, args);
            }
        }

        public event global::Spine.AnimationState.TrackEntryEventDelegate Event;

        public Skeleton Skeleton => skeleton;



        public float AnimationSpeed { get; set; } = 1;

        public float ScaleX { get; set; } = 1;
        public float ScaleY { get; set; } = 1;

        public bool UpdateEveryFrame => true;

        public bool Visible { get; set; } = true;

        IVisible IVisible.Parent
        {
            get
            {
                return this.Parent as IVisible;
            }
        }

        public bool AbsoluteVisible
        {
            get
            {
                if (this.Visible)
                {
                    var parentAsIVisible = this.Parent as IVisible;

                    if (parentAsIVisible == null || IgnoresParentVisibility)
                    {
                        return true;
                    }
                    else
                    {
                        // this is true, so return if the parent is visible:
                        return parentAsIVisible.AbsoluteVisible;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IgnoresParentVisibility
        {
            get;
            set;
        }
        SkeletonBounds bounds = new SkeletonBounds();

        #endregion

        /// <summary>
        /// Don't call this, instead call SpriteManager.RemoveDrawableBatch
        /// </summary>
        public void Destroy()
        {
            this.RemoveSelfFromListsBelongingTo();
        }

        public static SpineDrawableBatch FromFile(FilePath skeletonFile, FilePath atlasFile)
        {
            return FromFile(skeletonFile.FullPath, atlasFile.FullPath);
        }

        public static SpineDrawableBatch FromFile(string skeletonPath, string atlasPath)
        {
            Atlas atlas = new Atlas(atlasPath, new XnaTextureLoader(FlatRedBallServices.GraphicsDevice));
            return FromFile(skeletonPath, atlas);
        }

        public static SpineDrawableBatch FromFile(string skeletonPath, Atlas atlas)
        {
            if (atlas == null)
            {
                throw new ArgumentNullException(nameof(atlas));
            }
            SpineDrawableBatch toReturn = new SpineDrawableBatch();

            SkeletonData skeletonData;

            var skeletonExtension = FileManager.GetExtension(skeletonPath);

            if (skeletonExtension == "skel")
            {
                SkeletonBinary binary = new SkeletonBinary(atlas);
                binary.Scale = 1;
                skeletonData = binary.ReadSkeletonData(skeletonPath);
            }
            else
            {
                SkeletonJson json = new SkeletonJson(atlas);
                json.Scale = 1;
                skeletonData = json.ReadSkeletonData(skeletonPath);
            }

            toReturn.skeleton = new Skeleton(skeletonData);

            // set skin?
            //if (name == "goblins-pro") skeleton.SetSkin("goblin");

            // Define mixing between animations.
            AnimationStateData stateData = new AnimationStateData(toReturn.skeleton.Data);
            toReturn.AnimationState = new AnimationState(stateData);

            //if (name == "spineboy-ess")
            //{
            //    skeleton.SetAttachment("head-bb", "head"); // Activate the head BoundingBoxAttachment.

            //    stateData.SetMix("run", "jump", 0.2f);
            //    stateData.SetMix("jump", "run", 0.4f);

            //    // Event handling for all animations.
            //    state.Start += Start;
            //    state.End += End;
            //    state.Complete += Complete;
            //    state.Event += Event;

            //    state.SetAnimation(0, "run", false);
            //    TrackEntry entry = state.AddAnimation(0, "jump", false, 0);
            //    entry.End += End; // Event handling for queued animations.
            //    state.AddAnimation(0, "run", true, 0);
            //}
            //else if (name == "raptor-pro")
            //{
            //state.SetAnimation(0, "walk", true);
            //state.AddAnimation(1, "gun-grab", false, 2);
            //}
            //else if (name == "coin-pro")
            //{
            //    state.SetAnimation(0, "animation", true);
            //}
            //else if (name == "tank-pro")
            //{
            //    state.SetAnimation(0, "drive", true);
            //}
            //else
            //{
            //    state.SetAnimation(0, "walk", true);
            //}


            //headSlot = skeleton.FindSlot("head");
            return toReturn;
        }


        public void Update()
        {
            // January 21, 2024
            // Through testing this
            // is the set of values that
            // are needed for animations. Not
            // sure if we can get rid of some of
            // these calls from the Draw, but this
            // at least makes collision align with everything.
            skeleton.ScaleX = ScaleX;
            skeleton.ScaleY = ScaleY;
            state.Update(TimeManager.SecondDifference * AnimationSpeed);
            state.Apply(skeleton);
            skeleton.UpdateWorldTransform(Skeleton.Physics.None);
        }

        public void Draw(Camera camera)
        {
            if (!AbsoluteVisible) return;
            skeleton.ScaleX = ScaleX;
            skeleton.ScaleY = ScaleY;
            skeleton.X = (X) + camera.OrthogonalWidth / (2.0f);
            skeleton.Y = (-Y) + camera.OrthogonalHeight / (2.0f);
            state.Apply(skeleton);
            skeleton.UpdateWorldTransform(Skeleton.Physics.None);

            //SpineEffect.Parameters["World"].SetValue(Matrix.Identity);

            SpineManager.PrepareDraw(camera);
            var skeletonRenderer = SpineManager.SkeletonRenderer;

            skeletonRenderer.Begin();
            skeletonRenderer.Draw(skeleton);
            skeletonRenderer.End();

            //MouseState mouse = Mouse.GetState();
            //if (headSlot != null)
            //{
            //headSlot.G = 1;
            //headSlot.B = 1;
            //if (bounds.AabbContainsPoint(mouse.X, mouse.Y))
            //{
            //    BoundingBoxAttachment hit = bounds.ContainsPoint(mouse.X, mouse.Y);
            //    if (hit != null)
            //    {
            //        headSlot.G = 0;
            //        headSlot.B = 0;
            //    }
            //}
            //}
        }

        public void AddToManagers(Layer layer)
        {
            bool isAlreadyManaged = SpriteManager.ManagedPositionedObjects
                .Contains(this);

            // This allows AddToManagers to be called multiple times, so it can be added to multiple layers
            if (!isAlreadyManaged)
            {
                SpriteManager.AddPositionedObject(this);
            }

            SpriteManager.AddToLayer(this, layer);
        }

        public SpineDrawableBatch Clone()
        {
            var clone = new SpineDrawableBatch();
            clone.skeleton = new Skeleton(skeleton);
            clone.AnimationState = new AnimationState(this.AnimationState.Data);
            clone.bounds = new SkeletonBounds();
            return clone;
        }

        public void SetCollision(ShapeCollection shapeCollection, PositionedObject parent, bool createMissingShapes = false)
        {
            bounds.Update(skeleton, true);

            var count = bounds.BoundingBoxes.Count;

            for (int spinePolygonIndex = 0; spinePolygonIndex < count; spinePolygonIndex++)
            {
                var spineBoundingBox = bounds.BoundingBoxes.Items[spinePolygonIndex];
                var spinePolygon = bounds.Polygons.Items[spinePolygonIndex];

                var frbPolygon = shapeCollection.Polygons.FindByName(spineBoundingBox.Name);

                if (createMissingShapes && frbPolygon == null)
                {
                    frbPolygon = new FlatRedBall.Math.Geometry.Polygon();
                    frbPolygon.Name = spineBoundingBox.Name;
                    shapeCollection.Polygons.Add(frbPolygon);
                    frbPolygon.AttachTo(parent);
                }

                if (frbPolygon != null)
                {
                    if (frbPolygon.Points == null || (2 * frbPolygon.Points.Count - 1) != spineBoundingBox.Vertices.Length)
                    {
                        frbPolygon.Points = new Point[1 + spineBoundingBox.Vertices.Length / 2];
                    }

                    for (int i = 0; i < frbPolygon.Points.Count - 1; i++)
                    {
                        var worldX = spinePolygon.Vertices[i * 2];
                        var worldY = spinePolygon.Vertices[i * 2 + 1];

                        //frbPolygon.SetPoint(i, x*ScaleX, y*ScaleY);
                        frbPolygon.SetPoint(i, worldX - skeleton.X, -1 * worldY + skeleton.Y);

                    }
                    frbPolygon.SetPoint(frbPolygon.Points.Count - 1, frbPolygon.Points[0]);
                    if (!frbPolygon.IsClockwise())
                    {
                        frbPolygon.InvertPointOrder();
                    }

                    frbPolygon.ForceUpdateDependencies();
                }
            }

        }


        /// <summary>
        /// This method either finds an existing skin with the provided name, or it creates
        /// a new skin from the provided template
        /// </summary>
        /// <param name="templateName">The name of the template skin to base a new skin on</param>
        /// <param name="dynamicSkinName">The name of the dynamic skin to create</param>
        /// <returns>A copy of the template skin, or the dynamic skin if it was found.</returns>
        /// <exception cref="Exception">Thrown if the template skin was not found on the skeleton.</exception>
        public Skin GetOrCreateDynamicSkinFromTemplate(string templateName, string dynamicSkinName)
        {
            var dynamicSkin = Skeleton.Data.FindSkin(dynamicSkinName);
            if(dynamicSkin == null)
            {
                var templateSkin = Skeleton.Data.FindSkin(templateName);
                if (templateSkin == null)
                {
                    throw new Exception($"{templateName} does not exist on this skeleton!");
                }
                dynamicSkin = new Skin(dynamicSkinName);
                dynamicSkin.CopySkin(templateSkin);
            }
            
            return dynamicSkin;
        }

        /// <summary>
        /// Sets the provided atlas texture on the provided skin at the provided placeholder slot if
        /// all exist. Note that skin instances are shared across skeletons so changing the texture on
        /// a skin will apply to all creatures using that skin.
        /// 
        /// It is recommended to combine this method with GetOrCreateDynamicSkinFromTemplate in order to
        /// create unique runtime skins with customized placeholder slots.
        /// </summary>
        /// <param name="skin">The skin to update. Updates will apply to all instances using this skin!</param>
        /// <param name="placeholderName">The placeholder name to update on the skin.</param>
        /// <param name="atlasSource">The atlas containing the target texture.</param>
        /// <param name="atlasTextureName">The texture name on the provided atlas.</param>
        /// <param name="applyImmediately">Whether to immediately set the updated skin to this skeleton. Defaults to true.</param>
        /// <exception cref="Exception">Thrown if placeholder or atlas texture names don't exist on the skeleton or atlas.</exception>
        public void SetDynamicTextureOnSkin(Skin skin, string placeholderName, Atlas atlasSource, string atlasTextureName, bool applyImmediately = true)
        {
            var slot = skin.Attachments.Where(a => a.Name == placeholderName).FirstOrDefault();
            var region = slot.Attachment as RegionAttachment;
            if(region == null)
            {
                throw new Exception($"Could not find {placeholderName} on provided skin.");
            }

            var regionData = atlasSource.FindRegion(atlasTextureName);
            if(regionData == null)
            {
                throw new Exception($"Could not find region {atlasTextureName} on provided atlas.");
            }

            region.Region = regionData;
            region.UpdateRegion();

            if(applyImmediately)
            {
                Skeleton.SetSkin(skin);
                AnimationState.Apply(Skeleton);
            }
        }

        // Eventually we want to support IAnimatable interface, but for now we'll mimic it until we're ready

        public void PlayAnimation(string animationName, bool loop = true)
        {
            Animation foundAnimation = GetAnimationInternal(animationName);

            // see if it's already playing:
            foreach (var track in AnimationState.Tracks)
            {
                if (track.Animation != null && track.Animation.Name == foundAnimation.Name)
                {
                    return;
                }
            }

            AnimationState.SetAnimation(0, foundAnimation, loop: true);
        }

        public override void Pause(InstructionList instructions)
        {
            var oldSpeed = AnimationSpeed;
            instructions.Add(new DelegateInstruction(() => this.AnimationSpeed = oldSpeed));
            AnimationSpeed = 0;
        }

        private Animation GetAnimationInternal(string animationName)
        {
            var foundAnimation = AnimationState.Data.SkeletonData.Animations.Find(item => item.Name == animationName);

            if (foundAnimation == null)
            {
                var message = $"Could not find an animation named {animationName}.  Found:\n";
                foreach (var animation in AnimationState.Data.SkeletonData.Animations)
                {
                    message += "  " + animation.Name + "\n";
                }
                throw new ArgumentException(message);
            }

            return foundAnimation;
        }

        public async Task PlayAnimationAsync(string animationName)
        {
            Animation foundAnimation = GetAnimationInternal(animationName);
            AnimationState.SetAnimation(0, foundAnimation, loop: false);
            await TimeManager.DelaySeconds(foundAnimation.Duration);

        }
    }
}
