using System.Threading.Tasks;

namespace ItemManagement.Application.UseCaseInterfaces
{
    public interface ISellItemUseCase
    {
          Task ExecuteAsync(string cashierName, int itemId, int qtyToSell);
    }
}