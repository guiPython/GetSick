using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe TextWithNumber representa um texto no editor que tem um número junto
/// utilizado para facilitar o trabalho de conversão entre os dois mundos
/// </summary>
public class TextWithNumber
{
    private string template;
    private int value; // Valor do número
    private Text textWithNumber; // Componente de texto do gameObject

    /*
     * Método para ser utilizado na criação de um objeto com esse componente
     * como se fosse um construtor
     */
    public TextWithNumber(Text textComponent, string template, int initialValue)
    {
        this.textWithNumber = textComponent;
        this.template = template;
        this.SetValue(initialValue);
    }

    /*
     * Pega o valor
     */
    public int GetValue()
    {
        return this.value;
    }

    /*
     * Atribui um valor
     */
    public void SetValue(int value)
    {
        this.value = value;
        this.textWithNumber.text = string.Format(template, value);
    }

    /*
     * Definição do operador ++
     */
    public static TextWithNumber operator ++(TextWithNumber a)
    {
        a.SetValue(a.value + 1);
        return a;
    }

    /*
     * Definição do operador == para inteiros
     */
    public static bool operator ==(TextWithNumber a, int b)
    {
        return a.value == b;
    }

    /*
     * Definição do operador != para inteiros
     */
    public static bool operator !=(TextWithNumber a, int b)
    {
        return a.value != b;
    }

    /*
     * Definição do operador ==
     */
    public static bool operator ==(TextWithNumber a, TextWithNumber b)
    {
        return a.value == b.value;
    }

    /*
     * Definição do operador !=
     */
    public static bool operator !=(TextWithNumber a, TextWithNumber b)
    {
        return a.value != b.value;
    }

    /*
     * Definição do operador <
     */
    public static bool operator <(TextWithNumber a, TextWithNumber b)
    {
        return a.value < b.value;
    }

    /*
     * Definição do operador >
     */
    public static bool operator >(TextWithNumber a, TextWithNumber b)
    {
        return a.value > b.value;
    }

    /*
     * Definição do operador <=
     */
    public static bool operator <=(TextWithNumber a, TextWithNumber b)
    {
        return a.value <= b.value;
    }

    /*
     * Definição do operador >=
     */
    public static bool operator >=(TextWithNumber a, TextWithNumber b)
    {
        return a.value >= b.value;
    }

    /*
     * Definição da função de equalidade 
     */
    public override bool Equals(object obj)
    {
        return obj is TextWithNumber value &&
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