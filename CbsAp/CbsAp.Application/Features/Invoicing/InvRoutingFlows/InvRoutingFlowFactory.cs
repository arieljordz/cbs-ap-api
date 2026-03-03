using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows
{
    public static class InvRoutingFlowFactory
    {
        public static InvRoutingFlow Create(InvRoutingFlowDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new InvRoutingFlow
            {
                InvRoutingFlowName = dto.InvRoutingFlowName,
                EntityProfileID = dto.EntityProfileID,
                SupplierInfoID = dto.SupplierInfoID,
                MatchReference = dto.MatchReference,
                IsActive = dto.IsActive,
                Levels = dto.InvRoutingFlowLevels?.Select(l => new InvRoutingFlowLevels
                {
                    Level = l.Level,
                    RoleID = l.RoleID,
                }).ToList(),
            };
        }

        public static void Update(
               InvRoutingFlow invRoutingFlow,
               InvRoutingFlowDto dto)
        {
            ArgumentNullException.ThrowIfNull(invRoutingFlow);
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.InvRoutingFlowID != invRoutingFlow.InvRoutingFlowID)
                throw new InvalidOperationException("Mismatched InvRoutingFlowID in update operation.");

            // Update core properties
            invRoutingFlow.InvRoutingFlowName = dto.InvRoutingFlowName!;
            invRoutingFlow.EntityProfileID = dto.EntityProfileID;
            invRoutingFlow.SupplierInfoID = dto.SupplierInfoID;
            invRoutingFlow.MatchReference = dto.MatchReference;
            invRoutingFlow.IsActive = dto.IsActive;

            // Handle Levels update
            var incomingLevelMap = dto.InvRoutingFlowLevels!
                .Select(l => l.InvRoutingFlowLevelID)
                .ToHashSet() ?? [];

            var removedLevels = invRoutingFlow.Levels!
                .Where(l => !incomingLevelMap.Contains(l.InvRoutingFlowLevelID))
                .ToList();

            if (invRoutingFlow.Levels != null)
            {
                foreach (var level in removedLevels)
                {
                    invRoutingFlow.Levels.Remove(level);
                }

                foreach (var dtoLevel in dto.InvRoutingFlowLevels!)
                {
                    if (!invRoutingFlow.Levels.Any(l => l.InvRoutingFlowLevelID == dtoLevel.InvRoutingFlowLevelID))
                    {
                        invRoutingFlow.Levels.Add(new InvRoutingFlowLevels
                        {
                            RoleID = dtoLevel.RoleID,
                            Level = dtoLevel.Level
                        });
                    }
                }
            }
        }

        public static InvRoutingFlowDto GetInvRoutingFlowDto(InvRoutingFlow routingFlow)
        {
            ArgumentNullException.ThrowIfNull(routingFlow);
            return new InvRoutingFlowDto
            {
                InvRoutingFlowID = routingFlow.InvRoutingFlowID,
                InvRoutingFlowName = routingFlow.InvRoutingFlowName,
                EntityProfileID = routingFlow.EntityProfileID,
                SupplierInfoID = routingFlow.SupplierInfoID,
                IsActive = routingFlow.IsActive,
                MatchReference = routingFlow.MatchReference,
                InvRoutingFlowLevels = routingFlow.Levels!.Select(level => new InvRoutingFlowLevelsDto
                {
                    InvRoutingFlowLevelID = level.InvRoutingFlowLevelID,
                    InvRoutingFlowID = level.InvRoutingFlowID,
                    RoleID = level.RoleID,
                    Level = level.Level,
                }).OrderBy(l => l.Level).ToList()
            };
        }
    }
}