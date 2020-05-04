<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Project;
use Illuminate\Support\Facades\Storage;
use Illuminate\Support\Str;

class ProjectPhotoController extends Controller
{
    public function index(Project $project)
    {
        return $project->photos()->pluck('path');
    }

    public function store(Project $project)
    {
        $this->authorize('update', $project);

        $requestFile = request()->file('photo');
        $token       = Str::random(128);

        $disk = Storage::disk('public');
        $path = '/project-photos/';
        $disk->putFileAs($path, $requestFile, $token . '.' . $requestFile->extension());
        $fullPath = $path . $token . '.' . $requestFile->extension();

        $photo = $project->photos()->create([
            'team_id' => team()->id,
            'url'     => $disk->url($fullPath),
            'path'    => '/storage' . $fullPath,
            'token'   => $token,
        ]);

        return $this->success('Photo Successfully Uploaded', compact('photo'));
    }

    public function destroy(Project $project)
    {
        $this->authorize('update', $project);

        $photo = $project->photos()->where('token', '=', request('token'))->first();

        Storage::disk('public')->delete($photo->path);

        $photo->delete();

        return $this->success('Photo Successfully Deleted');
    }
}
