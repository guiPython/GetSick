using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe TextWithFloat representa um texto no editor que tem um número junto
/// utilizado para facilitar o trabalho de conversão entre os dois mundos
/// </summary>
public class TextWithFloat
{
    private string template;
    private float value; // Valor do número
    private Text textWithFloat; // Componente de texto do gameObject

    /*
     * Método para ser utilizado na criação de um objeto com esse componente
     * como se fosse um construtor
     */
    public TextWithFloat(Text textComponent, string template, float initialValue)
    {
        this.textWithFloat = textComponent;
        this.template = template;
        this.SetValue(initialValue);
    }

    /*
     * Pega o valor
     */
    public float GetValue()
    {
        return this.value;
    }

    /*
     * Atribui um valor
     */
    public void SetValue(float value)
    {
        this.value = value;
        this.textWithFloat.text = string.Format(template, value);
    }

    /*
     * Definição do operador ++
     */
    public static TextWithFloat operator ++(TextWithFloat a)
    {
        a.SetValue(a.value + 1);
        return a;
    }

    /*
     * Definição do operador == para floateiros
     */
    public static bool operator ==(TextWithFloat a, float b)
    {
        return a.value == b;
    }

    /*
     * Definição do operador != para floateiros
     */
    public static bool operator !=(TextWithFloat a, float b)
    {
        return a.value != b;
    }

    /*
     * Definição do operador ==
     */
    public static bool operator ==(TextWithFloat a, TextWithFloat b)
    {
        return a.value == b.value;
    }

    /*
     * Definição do operador !=
     */
    public static bool operator !=(TextWithFloat a, TextWithFloat b)
    {
        return a.value != b.value;
    }

    /*
     * Definição do operador <
     */
    public static bool operator <(TextWithFloat a, TextWithFloat b)
    {
        return a.value < b.value;
    }

    /*
     * Definição do operador >
     */
    public static bool operator >(TextWithFloat a, TextWithFloat b)
    {
        return a.value > b.value;
    }

    /*
     * Definição do operador <=
     */
    public static bool operator <=(TextWithFloat a, TextWithFloat b)
    {
        return a.value <= b.value;
    }

    /*
     * Definição do operador >=
     */
    public static bool operator >=(TextWithFloat a, TextWithFloat b)
    {
        return a.value >= b.value;
    }

    /*
     * Definição da função de equalidade 
     */
    public override bool Equals(object obj)
    {
        return obj is TextWithFloat value &&
               base.Equals(obj) &&
               this.value == value.value;
    }

    /*
     * Definição da operação de hashCode
     */
    public override int GetHashCode()
    {
        int hashCode = 1091060534;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + this.value.GetHashCode();
        return hashCode;
    }
}