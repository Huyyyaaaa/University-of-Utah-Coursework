close all;
clear;
clc;


%% Define the unknown function to interpolate
 f = @(x) 1./(1+25*x.^2);

% Setup different numbers of interpolation nodes/data sites.
Nd = [2,4,8,16,32,64,128,256];

% Setup the points at which we want to evaluate the interpolant
xe = linspace(-1,1,10000)'; %generate evenly-spaced points
ye = f(xe); %evaluate function

% Preallocate space for numerical solution
ye_num = zeros(length(ye),length(Nd));

elapsed = zeros(length(Nd),1);
%% Loop goes over different numbers of interpolation nodes
for it=1:length(Nd)
   x = chebextrema(-1,1,Nd(it)); %generate points using chebyshev extrema.
    y = f(x);
    
    % use barycentric lagrange interpolation
    
    tic;% timer
    
      ye_num = barycheb2(x,y,xe)';
%     ye_num = polyfit(x,y,Nd(it)); 
%     polyval(ye_num,xe);
    elapsed(it) = toc;

end

%% Now plot relative errors
figure
e2 = zeros(length(Nd),1);
for it=1:length(Nd)
    e2(it,1) = norm(ye_num(:,it)-ye)./norm(ye);
end
semilogy(elapsed,e2);
    