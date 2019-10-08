using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using MathFunctions;

namespace MatrixHierarchies
{
    class SceneObject
    {
        protected SceneObject parent = null;
        protected List<SceneObject> children = new List<SceneObject>();

        protected Matrix3 localTransform = new Matrix3();
        protected Matrix3 globalTransform = new Matrix3();

        public SceneObject()
        {

        }

        ~SceneObject()
        {
            if (parent != null)
            {
                parent.RemoveChild(this);
            }

            foreach (SceneObject so in children)
            {
                so.parent = null;
            }
        }

        // Transform related ---------------------------------------------------
        public Matrix3 LocalTransform
        {
            get { return localTransform;  }
        }

        public Matrix3 GlobalTransform
        {
            get { return globalTransform;  }
        }

        // Updates -------------------------------------------------------------
        public void Update(float deltaTime)
        {
            // Run OnUpdate, then update all children.
            OnUpdate(deltaTime);

            foreach (SceneObject child in children)
            {
                child.Update(deltaTime);
            }
        }

        public void Draw()
        {
            // Run OnDraw, then draw all children.
            OnDraw();

            foreach (SceneObject child in children)
            {
                child.Draw();
            }
        }

        public void UpdateTransform()
        {
            if (parent != null)
            {
                globalTransform = parent.globalTransform * localTransform;
            }
            else
            {
                globalTransform = localTransform;
            }

            foreach (SceneObject child in children)
            {
                child.UpdateTransform();
            }
        }

        // Transformations -----------------------------------------------------
        public void SetPosition(float x, float y)
        {
            localTransform.SetTranslation(x, y);
            UpdateTransform();
        }
        public void SetRotate(float radians)
        {
            localTransform.SetRotateZ(radians);
            UpdateTransform();
        }
        public void SetScale(float width, float height)
        {
            localTransform.SetScaled(width, height, 1);
            UpdateTransform();
        }
        public void Translate(float x, float y)
        {
            localTransform.Translate(x, y);
            UpdateTransform();
        }
        public void Rotate(float radians)
        {
            localTransform.RotateZ(radians);
            UpdateTransform();
        }
        public void Scale(float width, float height)
        {
            localTransform.Scale(width, height, 1);
            UpdateTransform();
        }

        // Parenting Operations ------------------------------------------------
        public SceneObject Parent
        {
            get { return parent;  }
        }

        // Childing Opperations -------------------------------------------------
        public void AddChild(SceneObject child)
        {
            // Check to make sure child doesnt already have a parent.
            Debug.Assert(child.parent == null);

            // Assign this as the parent and add child to the children list.
            child.parent = this;
            children.Add(child);
        }

        public void RemoveChild(SceneObject child)
        {
            if (children.Remove(child) == true)
            {
                child.parent = null;
            }
        }

        public int GetChildCount()
        {
            return children.Count;
        }

        public SceneObject GetChild(int index)
        {
            return children[index];
        }

        // Virtuals ------------------------------------------------------------
        public virtual void OnUpdate(float deltaTime)
        {

        }

        public virtual void OnDraw()
        {

        }
    }
}
