import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { FrequencyTypeService } from './frequency-type.service';

describe('FrequencyTypeService', () => {
  it('GETs frequency type list', async () => {
    const get = jest.fn().mockResolvedValue({ frequencyTypes: [] });
    const service = runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: { get } }] }),
      () => new FrequencyTypeService()
    );
    await service.list();
    expect(get).toHaveBeenCalledWith('api/v1.0/frequencyTypes');
  });
});
