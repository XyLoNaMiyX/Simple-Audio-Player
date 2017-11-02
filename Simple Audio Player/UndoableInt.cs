using System;
using System.Collections.Generic;

public class UndoableInt {
	readonly List<int> StoredValues = new List<int>();
	int Step = 1;
	
	public bool Undo(int times = 1) {
		int bStep = Step;
		for (int j = times; j > 0; j--) {
			Step++;
			int i = StoredValues.Count - Step;
			if (i < 0)
				i = StoredValues.Count - --Step;
			Value = StoredValues[i];
		}
		return bStep != Step;
	}
	
	public bool Redo(int times = 1) {
		int bStep = Step;
		for (int j = times; j > 0; j--) {
			Step--;
			int i = StoredValues.Count - Step;
			if (i >= StoredValues.Count)
				i = StoredValues.Count - ++Step;
			Value = StoredValues[i];
		}
		return bStep != Step;
	}
	
	public void SetValue(int value) {
		StoredValues.Add(value);
		Value = value;
		Step = 1;
	}
	
	public UndoableInt(int initialValue)
	{ SetValue(initialValue); }
	
	public static implicit operator UndoableInt(int initialValue)
	{ return new UndoableInt(initialValue); }
	
	public static implicit operator int(UndoableInt undoableInt)
	{ return undoableInt.Value; }
	
	public int Value { get; private set; }
	
	public bool Contains(int value)
	{ return StoredValues.Contains(value); }
	
	public int StepCount { get { return StoredValues.Count; } }
}