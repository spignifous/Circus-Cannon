/// <summary>
/// N�o precisa especificamente de nada aqui al�m do fato de ser persistente.
/// Eu gosto de manter um objeto principal que nunca � destruido, com sub-sistemas como filhos.
/// </summary>
public class Systems : PersistentSingleton<Systems>
{

}