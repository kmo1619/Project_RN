using System;
using System.Collections.Generic;

public static class StateDisplayNames
{
    private static readonly Dictionary<Type, string> names =
        new Dictionary<Type, string>
        {
            // Player
            { typeof(PlayerIdleState),              "IDLE"              },
            { typeof(PlayerMoveState),              "MOVE"              },
            { typeof(PlayerAttackStartupState),     "ATTACK STARTUP"    },
            { typeof(PlayerAttackRecoveryState),    "ATTACK RECOVERY"   },
            { typeof(PlayerParryStartupState),      "PARRY STARTUP"     },
            { typeof(PlayerParryActiveState),       "PARRY ACTIVE"      },
            { typeof(PlayerDashState),              "DASH"              },
            { typeof(PlayerHitStunState),           "HIT STUN"          },
            { typeof(PlayerDeadState),              "DEAD"              },

            // Enemy Base
            { typeof(EnemyIdleState),               "IDLE"              },
            { typeof(EnemyHitStunState),            "HIT STUN"          },
            { typeof(EnemyKnockdownState),          "KNOCKDOWN"         },
            { typeof(EnemyDeadState),               "DEAD"              },

            // ForgottenOne
            { typeof(ForgottenOneChaseState),           "CHASE"             },
            { typeof(ForgottenOneAttackReadyState),     "ATTACK READY"      },
            { typeof(ForgottenOneAttackRecoveryState),  "ATTACK RECOVERY"   },
        };

    public static string Get(IState state)
    {
        if (state == null)
            return "NONE";

        if (names.TryGetValue(state.GetType(), out string display))
            return display;

        return state.GetType().Name.ToUpper();
    }
}
