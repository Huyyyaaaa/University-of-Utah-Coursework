<spark-team-profile :user="user" :team="team" inline-template>
    <div>
        <div v-if="user && team">
            <!-- Update Team Photo -->
        @include('spark::settings.teams.update-team-photo')

        <!-- Update Team Name -->
        @include('spark::settings.teams.update-team-name')

        <!-- Update Team Details -->
            @if(Auth::user()->ownsTeam($team))
                <update-team-information :user="user" :team="team"></update-team-information>
            @endif
        </div>
    </div>
</spark-team-profile>
