namespace Blue.ECS
{
	public abstract class ManagedSystem
	{
		public virtual void Start() { }

		public virtual void Clean() { }

		public virtual void Update() { }

		public virtual void Render() { }
	}
}
