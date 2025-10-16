using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "ScriptableObjects/IntVariable", order = 2)]
public class IntVariable : Variable<int>
{

    public int previousHighestValue;
    public override void SetValue(int value)
    {
        if (value >= previousHighestValue) previousHighestValue = value;

        _value = value;
    }

    // overload
    public void SetValue(IntVariable value)
    {
        Debug.Log("setvalue called in intvarcs. set value to " + value.Value);
        SetValue(value.Value);
    }

    public void ApplyChange(int amount)
    {
        Debug.Log("applying change of " + amount);
        this.Value += amount;
    }

    public void ApplyChange(IntVariable amount)
    {
        ApplyChange(amount.Value);
    }

    public void ResetHighestValue()
    {
        previousHighestValue = 0;
    }

}