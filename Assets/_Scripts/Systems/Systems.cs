/// <summary>
/// Não precisa especificamente de nada aqui além do fato de ser persistente.
/// Eu gosto de manter um objeto principal que nunca é destruido, com sub-sistemas como filhos.
/// </summary>
public class Systems : PersistentSingleton<Systems>
{

}