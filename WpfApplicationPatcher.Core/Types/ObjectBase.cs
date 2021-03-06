﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using JetBrains.Annotations;

namespace WpfApplicationPatcher.Core.Types {
	public abstract class ObjectBase<TObject> where TObject : class {
		internal readonly TObject Instance;
		private readonly ConcurrentDictionary<string, object> values;

		protected ObjectBase(TObject instance) {
			Instance = instance;
			values = new ConcurrentDictionary<string, object>();
		}

		protected TValue GetOrCreate<TValue>(Func<TValue> value) {
			return (TValue)values.GetOrAdd(new StackTrace().GetFrame(1).GetMethod().Name, _ => value());
		}

		public override int GetHashCode() {
			return Instance?.GetHashCode() ?? 0;
		}
		public override bool Equals(object obj) {
			return obj is ObjectBase<TObject> that && Instance.Equals(that.Instance);
		}

		public static bool operator ==(ObjectBase<TObject> left, ObjectBase<TObject> right) {
			return IsNull(left) == IsNull(right) && (IsNull(left) || IsNull(left.Instance) == IsNull(right.Instance) && (IsNull(left.Instance) || left.Equals(right)));
		}
		public static bool operator !=(ObjectBase<TObject> left, ObjectBase<TObject> right) {
			return IsNull(left) != IsNull(right) || !IsNull(left) && (IsNull(left.Instance) != IsNull(right.Instance) || !IsNull(left.Instance) && !left.Equals(right));
		}

		[ContractAnnotation("null => true; notnull => false")]
		private static bool IsNull(object obj) {
			return obj?.Equals(null) != false;
		}
	}
}