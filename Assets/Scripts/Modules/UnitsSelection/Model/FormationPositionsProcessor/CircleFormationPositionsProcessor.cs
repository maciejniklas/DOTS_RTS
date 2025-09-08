using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS_RTS.Modules.UnitsSelection.Model.FormationPositionsProcessor
{
    [CreateAssetMenu(fileName = "CircleFormationPositionsProcessor", menuName = "DOTS_RTS/FormationPositionsProcessor/Circle")]
    public class CircleFormationPositionsProcessor : FormationPositionsProcessor
    {
        public override Vector3[] Process(Vector3 center, int size)
        {
            var floatParameters = new NativeArray<float>(2, Allocator.TempJob) { [0] = unitRadius, [1] = unitSpacing };
            var float3Parameters = new NativeArray<Vector3>(1, Allocator.TempJob) { [0] = center };
            var positions = new NativeArray<Vector3>(size, Allocator.TempJob);

            var job = new ComputePositionsJob
            {
                FloatParameters = floatParameters,
                Float3Parameters = float3Parameters,
                Positions = positions
            };

            var handle = job.ScheduleParallel(size, 64, default);
            handle.Complete();
            
            var result = new Vector3[size];
            positions.CopyTo(result);
                
            floatParameters.Dispose();
            float3Parameters.Dispose();
            positions.Dispose();

            return result;
        }
        
        private struct ComputePositionsJob : IJobFor
        {
            [ReadOnly] public NativeArray<float> FloatParameters;
            [ReadOnly] public NativeArray<Vector3> Float3Parameters;
            public NativeArray<Vector3> Positions;
            
            public void Execute(int index)
            {
                if (index <= 0)
                {
                    Positions[0] = Float3Parameters[0];
                    
                    return;
                }
                
                var layerIndex = Mathf.CeilToInt((index) / (2 * Mathf.PI));

                var minPositionIndexAtLayer = (int)(math.floor((layerIndex - 1) * math.PI2) + 1);
                var maxPositionIndexAtLayer = (int)(math.floor(layerIndex * math.PI2) + 1);
                var positionsCountAtLayer = maxPositionIndexAtLayer - minPositionIndexAtLayer;
                var unitRadius = FloatParameters[0];
                var unitSpacing = FloatParameters[1];
                var center = Float3Parameters[0];
                
                var angle = math.PI2 / positionsCountAtLayer * (index - minPositionIndexAtLayer);
                var radius = 2 * unitRadius + unitSpacing;
                var x = center.x + radius * Mathf.Cos(angle);
                var z = center.z + radius * Mathf.Sin(angle);
                
                Positions[index] = new Vector3(x, 0, z);
            }
        }
    }
}