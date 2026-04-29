import { readFileSync } from 'fs';
import { join } from 'path';

describe('chart-recorder.token (source)', () => {
  const ts = readFileSync(join(__dirname, 'chart-recorder.token.ts'), 'utf8');

  it('exports ChartRecorder interface with onAttach, onUpdateDataset, onDestroy', () => {
    expect(ts).toMatch(/interface ChartRecorder/);
    expect(ts).toMatch(/onAttach/);
    expect(ts).toMatch(/onUpdateDataset/);
    expect(ts).toMatch(/onDestroy/);
  });

  it('exports NOOP_CHART_RECORDER with empty implementations', () => {
    expect(ts).toMatch(/NOOP_CHART_RECORDER/);
  });

  it('exports CHART_RECORDER injection token with root factory defaulting to NOOP', () => {
    expect(ts).toMatch(/CHART_RECORDER/);
    expect(ts).toMatch(/InjectionToken/);
    expect(ts).toMatch(/factory.*NOOP_CHART_RECORDER/s);
  });
});

describe('ChartJsLineAdapter recorder integration (source)', () => {
  const ts = readFileSync(join(__dirname, 'chart-js-line.adapter.ts'), 'utf8');

  it('injects CHART_RECORDER token', () => {
    expect(ts).toMatch(/CHART_RECORDER/);
    expect(ts).toMatch(/inject\(CHART_RECORDER\)/);
  });

  it('calls recorder.onAttach before chart construction', () => {
    expect(ts).toMatch(/recorder\.onAttach/);
  });

  it('calls recorder.onUpdateDataset before chart update', () => {
    expect(ts).toMatch(/recorder\.onUpdateDataset/);
  });

  it('calls recorder.onDestroy before chart destroy', () => {
    expect(ts).toMatch(/recorder\.onDestroy/);
  });
});
