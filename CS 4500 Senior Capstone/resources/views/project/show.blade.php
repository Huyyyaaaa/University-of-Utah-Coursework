@extends('spark::layouts.app')

@section('content')
    <project-show :project="{{ json_encode($project) }}"></project-show>
@endsection
