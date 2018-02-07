namespace WpfApplicationPatcher.Types.Base {
	// ReSharper disable PossibleNullReferenceException

	public class ObjectBase<TObject> where TObject : class {
		public readonly TObject Instance;

		protected ObjectBase(TObject instance) {
			Instance = instance;
		}

		public override int GetHashCode() {
			return Instance?.GetHashCode() ?? 0;
		}
		public override bool Equals(object obj) {
			return obj is ObjectBase<TObject> that && Instance.Equals(that.Instance);
		}

		public static bool operator ==(ObjectBase<TObject> left, ObjectBase<TObject> right) {
			return IsNull(left?.Instance) == IsNull(right?.Instance) && (IsNull(left?.Instance) || left.Instance.Equals(right.Instance));
		}

		public static bool operator !=(ObjectBase<TObject> left, ObjectBase<TObject> right) {
			return IsNull(left?.Instance) != IsNull(right?.Instance) || !IsNull(left?.Instance) && !left.Instance.Equals(right.Instance);
		}

		private static bool IsNull(object obj) {
			return obj?.Equals(null) != false;
		}
	}
}